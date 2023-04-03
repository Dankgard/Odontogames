﻿﻿using System;
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
    public event Action<AuthResponse> OnAuthSuccess = delegate {  };

    // This event activates when there is an authentication error, that is to say
    // when the server returns an error
    // This var is a list of delegated methods that execute on success
    public event Action<Exception> OnAuthFail = delegate {  };

    // This event activates when an authentication petition is started, that is to say
    // when the petition is sent to the server
    // This var is a list of delegated methods that execute on success
    public event Action OnAuthStarted = delegate {  };

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
    private StrapiGroup[] groups = null;

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
    
    public void Login(string username, string password) {
        OnAuthStarted?.Invoke();        
        var jsonString = "{" +
                         "\"identifier\":\"" + username + "\"," +
                         "\"password\":\"" + password + "\"" +
                         "}";

        PostAuthRequest("api/auth/local", jsonString);
    }

    public void Register(string username, string name, string surname, string email, string password) {
        OnAuthStarted?.Invoke();
        var jsonString = "{" +
                         "\"username\":\"" + username + "\"," +
                         "\"email\":\"" + email + "\"," +
                         "\"password\":\"" + password + "\"," +
                         "\"Firstname\":\"" + name + "\"," +
                         "\"Lastname\":\"" + surname + "\"";
        jsonString += "}";

        PostAuthRequest("api/auth/local/register", jsonString);
    }

    public virtual void DeleteAccount() {
        OnAuthStarted?.Invoke();
        string endpoint = "api/users/" + userID;
        DeleteRequest(endpoint);
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

        if (name != "" && name != AuthenticatedUser.Firstname)
        {
            jsonString += "\"Firstname\":\"" + name + "\",";
        }

        if (surname != "" && surname != AuthenticatedUser.Lastname)
        {
            jsonString += "\"Lastname\":\"" + surname + "\",";
        }
        jsonString = jsonString.TrimEnd(',');
        jsonString += "}";

        Debug.Log(jsonString);

        if (jsonString != "{}")
        {
            string id = AuthenticatedUser.id.ToString();
            string endpoint = $"api/users/{id}";

            PutRequest(endpoint, jsonString);
            UpdateUser(endpoint);
        }
        else
        {
            Debug.Log("No changes submitted");
            return;
        }
    }

    public void CreateRole(string name)
    {
        OnAuthStarted?.Invoke();
        var jsonString = "{" +
                         "\"name\":\"" + name + "\"";
        jsonString += "}";

        string endpoint = "api/users-permissions/roles";

        PostAuthRequest(endpoint, jsonString);
    }

    public virtual void SetUserGroup(string group, StrapiUser user)
    {
        OnAuthStarted?.Invoke();

        var jsonString = AuthenticatedUser;
        jsonString.group = group;

        string endpoint = "api/users/" + user.id;

        //PutRequest(endpoint, jsonString);
    }

    public virtual void PutRequest(string endpoint, string jsonString, bool requieresAuth = true)
    {
        string url = BaseURL + endpoint;
        OnAuthStarted?.Invoke();

        if (requieresAuth && !IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            return;
        }

        RestClient.Put<AuthResponse>(url, jsonString).Then(authResponse =>
        {
            Debug.Log("PUT request succeeded!");
        })
        .Catch(err =>
        {
            // Handle error
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
    }

    public virtual void DeleteRequest(string endpoint)
    {
        if (!IsAuthenticated) { 
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

    // This method makes a HTTP POST request to the server, receiving an AuthResponse if the operation
    // is successful. A request of this type is used to create new resources in the server's data base
    public virtual void PostAuthRequest(string endpoint, string jsonString)
    {
        OnAuthStarted?.Invoke();
        RestClient.Post<AuthResponse>(BaseURL + endpoint, jsonString).Then(authResponse =>
        {
            OnAuthSuccess?.Invoke(authResponse);
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });


        GetUsersRequest("api/users");
        GetGroupsRequest("api/users-permissions/roles");
    }

    public virtual void UpdateUser(string endpoint, bool IsAuthenticated = true)
    {
        string url = BaseURL + endpoint;
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            return;
        }

        OnAuthStarted?.Invoke();
        RestClient.Get<StrapiUser>(url).Then(userResponse =>
        {
            AuthenticatedUser = userResponse;

        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
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

    public virtual void GetUsersRequest(string endpoint)
    {
        OnAuthStarted?.Invoke();
        RestClient.GetArray<StrapiUser>(BaseURL + endpoint).Then(response =>
        {
            UserResponse userResponse = new UserResponse()
            {
                users = response
            };
            users = userResponse.users;
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
    }

    public virtual void GetGroupsRequest(string endpoint)
    {
        OnAuthStarted?.Invoke();
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + userJWT;
        RestClient.GetArray<StrapiGroup>(BaseURL + endpoint).Then(response =>
        {
            UserResponseGroups userResponse = new UserResponseGroups()
            {
                groups = response
            };
            groups = userResponse.groups;
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
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
        return AuthenticatedUser.Firstname;
    }

    public String GetSurname()
    {
        return AuthenticatedUser.Lastname;
    }

    public StrapiUser[] getUsers()
    {
        return users;
    }

    public StrapiGroup[] GetGroups()
    {
        return groups;
    }
}
