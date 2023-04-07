using System;
using System.Collections;
using LitJson;
using StrapiForUnity;
using UnityEngine;
using Proyecto26;

[System.Serializable]
public class UserData
{
    public string username;
    public string email;
    public string password;
    public string Firstname;
    public string Lastname;
}

public class StrapiComponent : MonoBehaviour
{
    // this event acctivates when the authentication petition is successful
    // This var is a list of delegated methods that execute on success
    public event Action<AuthResponse> OnAuthSuccess = delegate { };

    // This event activates when there is an authentication error, that is to say
    // when the server returns an error
    // This var is a list of delegated methods that execute on success
    public event Action<Exception> OnAuthFail = delegate { };

    // This event activates when an authentication petition is started, that is to say
    // when the petition is sent to the server
    // This var is a list of delegated methods that execute on success
    public event Action OnAuthStarted = delegate { };

    // This event is activated when no JWT is found to be stored in the app
    // This var is a list of delegated methods that execute on success
    public event Action NoStoredJWT = delegate { };

    public event Action deleteAccount = delegate { };

    [Tooltip("The root URL of your Strapi server. For example: http://localhost:1337")]
    public string BaseURL;

    [Tooltip("The secret key used for creating the JWT on your Strapi server. This is used to check that the stored JWT is valid. The secret can be found in your Strapi installation, at 'strapi/extensions/users-permissions/config/jwt.js'")]
    public string JWTSecret;

    public StrapiUser AuthenticatedUser;
    public bool IsAuthenticated = false;
    public string ErrorMessage;

    private string userJWT = "";
    private string userID = "";

    private StrapiUser[] users = null;
    private StrapiTeamsData[] teams = null;

    //private StrapiRole[] roles;

    public int profesorRoleID = 10;
    public int studentRoleID = 11;
    private StrapiUserTeam strapiUserTeam;

    // Singleton
    public static StrapiComponent _instance;

    protected virtual void Awake()
    {
        if (!BaseURL.EndsWith("/"))
        {
            BaseURL += "/";
        }

        // Singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
                Destroy(gameObject);
        }

        OnAuthSuccess += OnAuthSuccessHandler;
    }

    // This method invokes OnAuthSucces with the AuthResponse parameter
    public void TriggerAuthSuccessEvent(AuthResponse authResponse)
    {
        OnAuthSuccess.Invoke(authResponse);
    }

    // This method invokes OnAuthFail with an Exception parameter
    public void TriggerAuthFailEvent(Exception exception)
    {
        OnAuthFail.Invoke(exception);
    }

    // This method invokes OnAuthStarted with no parameters. Only used to
    // notify that the authentication process has begun
    public void TriggerOnAuthStartedEvent()
    {
        OnAuthStarted.Invoke();
    }

    private void checkForSavedJWT()
    {
        if (userJWT != "" && isValidJWT(userJWT))
        {
            Debug.Log("Login with JWT");
            LoginWithJwt(userJWT);
        }
        else
        {
            Debug.Log("Login without JWT");
            NoStoredJWT?.Invoke();
        }
    }

    private bool isValidJWT(string jwt)
    {
        if (JWTSecret == "")
        {
            Debug.LogError("Couldn't validate stored JWT. You should consider setting your Strapi Component JWT secret.");
            return true;
        }

        try
        {
            JsonData jsonPayload = JWT.JsonWebToken.DecodeToObject(userJWT, JWTSecret);
            DateTime exp = ConvertFromUnixTimestamp(double.Parse(jsonPayload["exp"].ToString()));
            if (exp > DateTime.UtcNow.AddMinutes(1))
            {
                return true;
            }
            else // the token has expired
            {
                Debug.LogError("The token has expired. Deleting.");
            }
        }
        catch (JWT.SignatureVerificationException)
        {
            Debug.LogError("Invalid Strapi token");
        }

        return false;
    }

    /// <summary>
    /// Converts a Unix timestamp into a System.DateTime
    /// </summary>
    /// <param name="timestamp">The Unix timestamp in milliseconds to convert, as a double</param>
    /// <returns>DateTime obtained through conversion</returns>
    public static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return origin.AddSeconds(timestamp);
    }

    // This method is used to login with an authentication token
    public virtual void LoginWithJwt(string jwt)
    {
        OnAuthStarted?.Invoke();
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + jwt;
        RestClient.Get<StrapiUser>(BaseURL + "api/users/me").Then(response =>
        {
            AuthResponse authResponse = new AuthResponse()
            {
                jwt = jwt,
                user = response
            };
            OnAuthSuccess?.Invoke(authResponse);
        }).Catch(err =>
        {
            Debug.Log("Error logging with JWT");
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
    }

    // AQUI EMPIEZAN LOS METODOS DE USUARIO
    #region USER_SERVER_FUNCTIONS
    #region RegisterFunctions
    public void Register(string username, string name, string surname, string email, string password, bool isTeacher = false)
    {
        StartCoroutine(RegisterCoroutine(username, name, surname, email, password, isTeacher));
    }

    public IEnumerator RegisterCoroutine(string username, string name, string surname, string email, string password, bool isTeacher)
    {
        OnAuthStarted?.Invoke();
        var jsonString = "{" +
                         "\"username\":\"" + username + "\"," +
                         "\"email\":\"" + email + "\"," +
                         "\"password\":\"" + password + "\"," +
                         "\"firstname\":\"" + name + "\"," +
                         "\"lastname\":\"" + surname + "\",";

        jsonString = jsonString.TrimEnd(',');
        jsonString += "}";

        yield return PostAuthRequest("api/auth/local/register", jsonString);

        if (isTeacher)
        {
            yield return ChangeUserRole(profesorRoleID, userID);
        }
    }

    public IEnumerator ChangeUserRole(int roleID, string userID)
    {
        string endpoint = $"api/users/{userID}";

        var jsonString = "{" + "\"role\":\"" + roleID + "\",";

        jsonString = jsonString.TrimEnd(',');
        jsonString += "}";

        yield return PutRequest(endpoint, jsonString);
    }
    #endregion

    #region LoginAndProfileFunctions
    public void Login(string username, string password)
    {
        OnAuthStarted?.Invoke();
        var jsonString = "{" +
                         "\"identifier\":\"" + username + "\"," +
                         "\"password\":\"" + password + "\"" +
                         "}";

        string endpoint = "api/auth/local";
        StartCoroutine(PostAuthRequest(endpoint, jsonString));
    }

    public virtual void EditProfile(string username = "", string email = "", string password = "", string name = "", string surname = "")
    {
        OnAuthStarted?.Invoke();
        string jsonString = "{";
        if (username != "" && username != AuthenticatedUser.username)
        {
            jsonString += "\"username\":\"" + username + "\",";
        }

        if (email != "" && email != AuthenticatedUser.email)
        {
            jsonString += "\"email\":\"" + email + "\",";
        }

        if (password != "" && password != AuthenticatedUser.password)
        {
            jsonString += "\"password\":\"" + password + "\",";
        }

        if (name != "" && name != AuthenticatedUser.firstname)
        {
            jsonString += "\"firstname\":\"" + name + "\",";
        }

        if (surname != "" && surname != AuthenticatedUser.lastname)
        {
            jsonString += "\"lastname\":\"" + surname + "\",";
        }

        jsonString = jsonString.TrimEnd(',');
        jsonString += "}";

        if (jsonString != "{}")
        {
            string id = AuthenticatedUser.id.ToString();
            string endpoint = $"api/users/{id}";

            StartCoroutine(PutRequest(endpoint, jsonString, () => StartCoroutine(GetUpdatedUserData(endpoint))));
        }
        else
        {
            Debug.Log("No changes submitted");
            return;
        }
    }

    #endregion

    #region StrapiUserTeam_functions
    public void CreateStrapiUserTeam(string groupName)
    {
        var jsonString = "{" +
                        "\"teamname\":\"" + groupName + "\"," +
                        "\"creator\":\"" + AuthenticatedUser.username + "\",";
        jsonString = jsonString.TrimEnd(',');
        jsonString += "}";

        string endpoint = "api/teams";

        PostGroupRequest(endpoint, jsonString);
    }

    public virtual void GetStrapiUserTeamsList()
    {
        string endpoint = "api/teams?populate=members";
        GetStrapiUserTeamsFromServer(endpoint);
    }

    public void GetStrapiUserTeamData(int id)
    {
        string endpoint = $"api/teams/{id}?populate=members";
        GetGroupDataRequest(endpoint);
    }

    public virtual void DeleteAccount()
    {
        OnAuthStarted?.Invoke();
        string endpoint = "api/users/" + userID;
        DeleteRequest(endpoint);
    }
    #endregion

    #region StrapiUser_methods
    public void GetListOfUsersFromServer()
    {
        StartCoroutine(GetListOfUsersFromServerCoroutine());
    }

    public IEnumerator GetListOfUsersFromServerCoroutine()
    {
        string endpoint = "api/users";
        yield return StartCoroutine(GetUsersRequest(endpoint));
    }

    public void GetListOfTeamsFromServer()
    {
        StartCoroutine(GetListOfTeamsFromServerCoroutine());
    }

    public IEnumerator GetListOfTeamsFromServerCoroutine()
    {
        string endpoint = "api/teams";
        yield return StartCoroutine(GetStrapiUserTeamsFromServer(endpoint));
    }

    #endregion
    #endregion

    // This method is a success handler for an authentication request. It receives an
    // authenticated response, an object that contains a JWT and a StrapiUser structure
    // that contains relevant data about the strapi user in question
    // we save both the user in the AuthenticatedUser var, the JWT in the userJWT var
    // and the id in the userID var
    // finally, we set the default request headers property Authorization with the
    // authorization type Bearer and the user's JWT
    public void OnAuthSuccessHandler(AuthResponse authResponse)
    {
        AuthenticatedUser = authResponse.user;
        userJWT = authResponse.jwt;
        userID = authResponse.user.id.ToString();
        IsAuthenticated = true;

        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + userJWT;
        Debug.Log($"Successfully authenticated. Welcome {AuthenticatedUser.username}");
        Debug.Log(userJWT);
    }

    #region POST_request_functions
    public IEnumerator PostAuthRequest(string endpoint, string jsonString)
    {
        AuthResponse response = null;

        OnAuthStarted?.Invoke();
        var request = RestClient.Post<AuthResponse>(BaseURL + endpoint, jsonString).Then(authResponse =>
        {
            response = authResponse;
            OnAuthSuccess?.Invoke(authResponse);
            Debug.Log(1);
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => response != null);
    }

    public virtual void PostGroupRequest(string endpoint, string jsonString)
    {
        string url = BaseURL + endpoint;
        OnAuthStarted?.Invoke();
        RestClient.Post<StrapiUserTeam>(url, jsonString).Then(response =>
        {
            strapiUserTeam = response;
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
    }
    #endregion

    #region PUT_request_functions
    public IEnumerator PutRequest(string endpoint, string jsonString, Action onSuccess = null, bool requieresAuth = true)
    {
        if (requieresAuth && !IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        AuthResponse response = null;
        string url = BaseURL + endpoint;

        OnAuthStarted?.Invoke();

        RestClient.Put<AuthResponse>(url, jsonString).Then(authResponse =>
        {
            response = authResponse;
            onSuccess?.Invoke();
            Debug.Log("PUT request succeeded!");
        })
        .Catch(err =>
        {
            // Handle error
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => response != null);
    }
    #endregion

    #region GET_request_functions
    public IEnumerator GetUpdatedUserData(string endpoint, bool IsAuthenticated = true)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        StrapiUser response = null;
        string url = BaseURL + endpoint;

        OnAuthStarted?.Invoke();
        RestClient.Get<StrapiUser>(url).Then(userResponse =>
        {
            AuthenticatedUser = userResponse;
            response = userResponse;

        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => response != null);
    }

    public IEnumerator GetUsersRequest(string endpoint)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        StrapiUser[] response = null;
        string url = BaseURL + endpoint;

        OnAuthStarted?.Invoke();
        RestClient.GetArray<StrapiUser>(url).Then(userResponse =>
        {
            users = userResponse;
            response = userResponse;
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => response != null);
    }

    public virtual int GetUserCount(string endpoint)
    {
        OnAuthStarted?.Invoke();
        RestClient.Get<int>(BaseURL + endpoint).Then(response =>
        {
            //UserCountResponse userCountResponse = new UserCountResponse()
            //{
            //    count = response
            //};
            return 3;
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");

            return -2;
        });

        return -1;
    }

    public virtual void GetAuthRequest(string endpoint)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            return;
        }

        OnAuthStarted?.Invoke();
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + userJWT;
        RestClient.Get<AuthResponse>(BaseURL + endpoint).Then(authResponse =>
        {
            OnAuthSuccess?.Invoke(authResponse);
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
    }

    public IEnumerator GetStrapiUserTeamsFromServer(string endpoint)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        StrapiUserTeamListResponse teamResponse = null;
        string url = BaseURL + endpoint;
        OnAuthStarted?.Invoke();
        RestClient.Get<StrapiUserTeamListResponse>(url).Then(response =>
        {
            teams = response.data;
            teamResponse = response;
            Debug.Log("Teams received successfully");
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => teamResponse != null);
    }

    public virtual void GetGroupDataRequest(string endpoint)
    {
        string url = BaseURL + endpoint;
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            return;
        }

        OnAuthStarted?.Invoke();
        RestClient.Get<StrapiUserTeam>(url).Then(response =>
        {
            strapiUserTeam = response;
            Debug.Log("Group data response successful");
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
    }
    #endregion

    #region DELETE_request_functions
    public virtual void DeleteRequest(string endpoint)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            return;
        }

        RestClient.Delete(BaseURL + endpoint).Then(response =>
        {
            Debug.Log($"Successfully deleted data at {endpoint}");
        })
        .Catch(err =>
        {
            Debug.LogError($"Error deleting data at {endpoint}: {err}");
        });

        //RestClient.Delete(BaseURL + endpoint);
        //userID = "";
    }
    #endregion

    #region Getter_functions
    public String GetUsername()
    {
        return AuthenticatedUser.username;
    }

    public String GetEmail()
    {
        return AuthenticatedUser.email;
    }

    public String GetPassword()
    {
        return AuthenticatedUser.password;
    }

    public String GetName()
    {
        return AuthenticatedUser.firstname;
    }

    public String GetSurname()
    {
        return AuthenticatedUser.lastname;
    }

    public StrapiUser[] GetUsers()
    {
        return users;
    }

    public StrapiTeamsData[] GetTeams()
    {
        return teams;
    }

    public StrapiUserTeam GetGroup(int id)
    {
        GetStrapiUserTeamData(id);

        return strapiUserTeam;
    }
    #endregion
}
