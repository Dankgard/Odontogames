using System;
using System.Collections;
using LitJson;
using StrapiForUnity;
using UnityEngine;
using Proyecto26;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

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
    private event Action NoStoredJWT = delegate { };

    [Tooltip("The root URL of your Strapi server. For example: http://localhost:1337")]
    public string BaseURL;

    [Tooltip("The secret key used for creating the JWT on your Strapi server. This is used to check that the stored JWT is valid. The secret can be found in your Strapi installation, at 'strapi/extensions/users-permissions/config/jwt.js'")]
    public string JWTSecret;

    private StrapiUser authenticatedUser;
    private bool IsAuthenticated = false;
    private string ErrorMessage;

    public string TEACHER_SECRET_PASSWORD = "1234";
    private bool userIsTeacher = false;

    private string userJWT = "";
    private string userID = "";

    private StrapiUser[] users = null;
    private StrapiTeamsData[] teams = null;

    public int profesorRoleID;
    public int studentRoleID;
    private StrapiUserTeam strapiUserTeam;

    // Instancia
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

    public void GetNotesFromServer()
    {
        StartCoroutine(GetNotesFromServerCoroutine("api/users"));
    }

    private IEnumerator GetNotesFromServerCoroutine(string endpoint)
    {
        yield return GetListOfUsersFromServerCoroutine(endpoint);
        Document document = new Document();
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "prueba.pdf");
        PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.OpenOrCreate));
        document.Open();

        //for (int i = 0; i < users.Length; i++)
        //{
        //    string line = "Usuario " + users[i].id + " " + users[i].firstname + " " + users[i].lastname +
        //        " " + users[i].username + " " + users[i].email + ": " + users[i].score + " puntos.\n";
        //    Paragraph paragraph = new Paragraph(line);
        //    document.Add(paragraph);
        //}        

        document.Close();
    }

    // AQUI EMPIEZAN LOS METODOS DE USUARIO
    #region USER_SERVER_FUNCTIONS
    #region RegisterFunctions
    public void Register(string username, string name, string surname, string email, string password, bool isTeacher = false, string specialPassword = "")
    {
        if (isTeacher)
        {
            if (specialPassword == TEACHER_SECRET_PASSWORD)
            {
                StartCoroutine(RegisterCoroutine(username, name, surname, email, password, true));
                userIsTeacher = true;
            }
            else Debug.Log("Wrong secret password");
        }
        else
        {
            StartCoroutine(RegisterCoroutine(username, name, surname, email, password, false));
        }
    }

    public IEnumerator RegisterCoroutine(string username, string name, string surname, string email, string password, bool isTeacher)
    {
        OnAuthStarted?.Invoke();
        var jsonObj = new JObject(
            new JProperty("username", username),
            new JProperty("email", email),
            new JProperty("password", password),
            new JProperty("firstname", name),
            new JProperty("lastname", surname),
            new JProperty("userrole", "estudiante")
        );

        string jsonString = jsonObj.ToString();
        yield return PostAuthRequest("api/auth/local/register", jsonString);

        if (isTeacher)
        {
            yield return ChangeUserRole(profesorRoleID, userID);
        }
        else yield break;
    }

    public IEnumerator ChangeUserRole(int roleID, string userID)
    {
        string endpoint = $"api/users/{userID}";

        var jsonObject = new JObject();
        jsonObject.Add("role", new JValue(roleID));
        jsonObject.Add("userrole", "profesor");

        string jsonString = jsonObject.ToString();

        yield return PutRequest(endpoint, jsonString);
    }
    #endregion

    #region LoginAndProfileFunctions
    public void Login(string username, string password)
    {
        OnAuthStarted?.Invoke();

        var jsonObject = new JObject(
            new JProperty("identifier", username),
            new JProperty("password", password)
        );
        var jsonString = jsonObject.ToString();

        string endpoint = "api/auth/local";
        StartCoroutine(PostAuthRequest(endpoint, jsonString));
    }

    public virtual void EditProfile(string username, string email, string name, string surname)
    {
        OnAuthStarted?.Invoke();
        JObject jsonObject = new JObject();

        if (!string.IsNullOrEmpty(username) && username != authenticatedUser.username)
        {
            jsonObject.Add("username", username);
        }

        if (!string.IsNullOrEmpty(email) && email != authenticatedUser.email)
        {
            jsonObject.Add("email", email);
        }

        if (!string.IsNullOrEmpty(name) && name != authenticatedUser.firstname)
        {
            jsonObject.Add("firstname", name);
        }

        if (!string.IsNullOrEmpty(surname) && surname != authenticatedUser.lastname)
        {
            jsonObject.Add("lastname", surname);
        }

        if (jsonObject.Count > 0)
        {
            string id = authenticatedUser.id.ToString();
            string endpoint = $"api/users/{id}";

            string jsonString = JsonConvert.SerializeObject(jsonObject);

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
    public void CreateStrapiUserTeam(string groupName, List<int> members)
    {
        StartCoroutine(CreateStrapiUserTeamCoroutine(groupName, members));
    }
    public IEnumerator CreateStrapiUserTeamCoroutine(string groupName, List<int> members)
    {
        int score = 0;
        foreach (int memberId in members)
        {
            int userIndex = Array.FindIndex(users, user => user.id == memberId);
            if (userIndex >= 0)
            {
                score += GetUserTotalPoints(userIndex);
            }
        }

        var teamObject = new JObject(
            new JProperty("teamname", groupName),
            new JProperty("creator", authenticatedUser.username),
            new JProperty("numplayers", new JValue(members.Count)),
            new JProperty("teamscore", new JValue(score)),
            new JProperty("members", JArray.FromObject(members)));

        string jsonString = JsonConvert.SerializeObject(new { data = teamObject });
        string endpoint = "api/teams";

        yield return StartCoroutine(PostGroupRequest(endpoint, jsonString));
        // Por alguna razon no se actualiza bien la relacion
        UpdateSelectedPlayersCoroutine(members);
    }

    private void UpdateSelectedPlayersCoroutine(List<int> members)
    {
        for (int i = 0; i < members.Count; i++)
        {
            var userObject = new JObject(
                new JProperty("group", strapiUserTeam.teamname));
            string userString = JsonConvert.SerializeObject(userObject);

            int userId = members[i];
            string endpoint = $"api/users/{userId}";

            if (userId == authenticatedUser.id)
            {
                StartCoroutine(PutRequest(endpoint, userString, () => StartCoroutine(GetUpdatedUserData(endpoint))));
            }
            else StartCoroutine(PutRequest(endpoint, userString));
        }
    }

    public void UpdateStrapiUserTeam(int teamID, List<int> members, bool addingPlayers)
    {
        StartCoroutine(UpdateStrapiUserTeamCoroutine(teamID, members, addingPlayers));
    }

    public IEnumerator UpdateStrapiUserTeamCoroutine(int teamID, List<int> members, bool addingPlayers) {
        string endpoint = $"api/teams/{teamID}?populate=members";
        Debug.Log("AAAAAAAAAAAAAAA");
        yield return GetGroupDataRequest(endpoint);

        int newGroupSize = strapiUserTeam.numplayers;
        if (!addingPlayers) newGroupSize -= members.Count;
        else newGroupSize += members.Count;

        if (newGroupSize < 0) newGroupSize = 0;

        int newScore = strapiUserTeam.teamscore;

        List<int> copy = new List<int>(members);
        for (int i = 0; i < users.Length; i++)
        {
            Debug.Log(users[i].id);
            int id = users[i].id;
            if (members.Contains(id))
            {
                if (!addingPlayers) newScore -= GetUserTotalPoints(users[i]);
                else newScore += GetUserTotalPoints(users[i]);
                copy.Remove(id);
            }
            if (copy.Count <= 0)
                break;
        }
        if (newScore < 0) newScore = 0;

        List<int> newMemberList = new List<int>();
        if (!addingPlayers)
        {
            for (int j = 0; j < strapiUserTeam.members.data.Length; j++)
            {
                int id = strapiUserTeam.members.data[j].id;
                if (!members.Contains(id))
                {
                    newMemberList.Add(id);
                }
            }
        }
        else
        {
            Debug.Log(strapiUserTeam.members.data);
            for (int j = 0; j < strapiUserTeam.members.data.Length; j++)
            {
                int id = strapiUserTeam.members.data[j].id;
                members.Add(id);
            }
            newMemberList = members;
        }

        var teamObject = new JObject(
            new JProperty("numplayers", new JValue(newGroupSize)),
            new JProperty("teamscore", new JValue(newScore)),
            new JProperty("members", JArray.FromObject(newMemberList)));

        string jsonString = JsonConvert.SerializeObject(new { data = teamObject });

        yield return StartCoroutine(PutGroupRequest(endpoint, jsonString));

        string group;
        if (!addingPlayers) group = "None";
        else group = strapiUserTeam.teamname;

        var userObject = new JObject(
            new JProperty("group", group));
        jsonString = JsonConvert.SerializeObject(userObject);
        for (int i = 0; i < members.Count; i++)
        {
            endpoint = $"api/users/{members[i]}";

            if (members[i] == int.Parse(userID))
            {
                StartCoroutine(PutRequest(endpoint, jsonString, () => StartCoroutine(GetUpdatedUserData(endpoint))));
            }
            else StartCoroutine(PutRequest(endpoint, jsonString));
        }
    }

    public void GetStrapiUserTeamsList()
    {
        string endpoint = "api/teams?populate=members";
        StartCoroutine(GetStrapiUserTeamsFromServer(endpoint));
    }

    public IEnumerator GetStrapiUserTeamData(int id)
    {
        string endpoint = $"api/teams/{id}?populate=members";
        yield return GetGroupDataRequest(endpoint);
    }
    public IEnumerator GetStrapiUserFreePlayers(int id)
    {
        string endpoint = $"api/users";
        Debug.Log(endpoint);
        yield return GetUsersRequest(endpoint);
    }
    #endregion

    #region StrapiUser_methods
    public void GetListOfUsersFromServer(string endpoint)
    {
        StartCoroutine(GetListOfUsersFromServerCoroutine(endpoint));
    }

    public IEnumerator GetListOfUsersFromServerCoroutine(string endpoint)
    {
        yield return StartCoroutine(GetUsersRequest(endpoint));
    }

    public void GetListOfTeamsFromServer()
    {
        StartCoroutine(GetListOfTeamsFromServerCoroutine());
    }

    public IEnumerator GetListOfTeamsFromServerCoroutine()
    {
        string endpoint = "api/teams?populate=members";
        yield return StartCoroutine(GetStrapiUserTeamsFromServer(endpoint));
    }

    public void UpdatePlayerScore(GameManager.Minigame_Score score, string minigame)
    {
        StartCoroutine(UpdatePlayerScoreCoroutine(score, minigame));
    }

    private IEnumerator UpdatePlayerScoreCoroutine(GameManager.Minigame_Score score, string minigame)
    {
        var jsonObject = new JObject(
            new JProperty(minigame, (score.correctAnswers - score.wrongAnswers + score.bonusPoints))
        );
        var jsonString = jsonObject.ToString();

        string endpoint = $"api/users/{userID}?populate=team";

        yield return PutRequest(endpoint, jsonString);
        yield return GetUpdatedUserData(endpoint);

        if (authenticatedUser.team != null)
        {
            Debug.Log("Updating player team");
            // TODO metodo para actualizar la puntuacion del grupo strapi
        }
    }

    #endregion

    #region Delete_functions
    public virtual void DeleteAccount()
    {
        OnAuthStarted?.Invoke();
        string endpoint = $"api/users/{authenticatedUser.id}";
        StartCoroutine(DeleteRequest(endpoint));
    }
    public virtual void DeleteUserAccount(int id)
    {
        OnAuthStarted?.Invoke();
        string endpoint = $"api/users/{id}";
        StartCoroutine(DeleteRequest(endpoint));
    }

    public void DeleteGroup(int id)
    {
        OnAuthStarted?.Invoke();
        string endpoint = $"api/teams/{id}";

        StrapiUserTeam team = GetGroupByID(id);

        for (int i = 0; i < team.members.data.Length; i++)
        {
            int userId = team.members.data[i].id;
            string userEndPoint = $"api/users/{userId}";

            var userObject = new JObject(
             new JProperty("group", "None"));
            
            string jsonString = JsonConvert.SerializeObject(userObject);

            StartCoroutine(PutRequest(userEndPoint, jsonString));
        }

        StartCoroutine(DeleteRequest(endpoint));
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
    private void OnAuthSuccessHandler(AuthResponse authResponse)
    {
        authenticatedUser = authResponse.user;
        userJWT = authResponse.jwt;
        userID = authResponse.user.id.ToString();
        IsAuthenticated = true;
        if (authenticatedUser.userrole == "profesor")
        {
            userIsTeacher = true;
        }

        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + userJWT;
        Debug.Log($"Successfully authenticated. Welcome {authenticatedUser.username}");
        Debug.Log(userJWT);

        StartCoroutine(GetUpdatedUserData($"api/users/{userID}"));
    }

    #region POST_request_functions
    private IEnumerator PostAuthRequest(string endpoint, string jsonString) {
        AuthResponse response = null;
        OnAuthStarted?.Invoke();
        var request = RestClient.Post<AuthResponse>(BaseURL + endpoint, jsonString).Then(authResponse =>
        {
            response = authResponse;
            OnAuthSuccess?.Invoke(authResponse);
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => response != null);
    }

    private IEnumerator PostGroupRequest(string endpoint, string jsonString)
    {
        strapiUserTeam = null;
        string url = BaseURL + endpoint;

        OnAuthStarted?.Invoke();
        RestClient.Post<StrapiUserTeamResponse>(url, jsonString).Then(strapiResponse =>
        {
            strapiUserTeam = strapiResponse.data.attributes;
            strapiUserTeam.id = int.Parse(strapiResponse.data.id);
            StartCoroutine(GetStrapiUserTeamData(strapiUserTeam.id));
        }).Catch(err =>
        {
            Debug.LogError(err);
            Debug.LogError($"POST request {err}");
        });

        yield return new WaitUntil(() => strapiUserTeam != null);
    }
    #endregion

    #region PUT_request_functions
    private IEnumerator PutRequest(string endpoint, string jsonString, Action onSuccess = null, bool requieresAuth = true)
    {
        AuthResponse response = null;
        string url = BaseURL + endpoint;

        Debug.Log(url);
        Debug.Log(jsonString);
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
            Debug.LogError("PUT request error");
        });

        yield return new WaitUntil(() => response != null);
    }

    private IEnumerator PutGroupRequest(string endpoint, string jsonString)
    {
        StrapiTeamsData response = null;
        string url = BaseURL + endpoint;

        Debug.Log(url);
        Debug.Log(jsonString);
        OnAuthStarted?.Invoke();
        RestClient.Put<StrapiTeamsData>(url, jsonString).Then(authResponse =>
        {
            response = authResponse;
            Debug.Log("PUT request succeeded!");
        })
        .Catch(err =>
        {
            // Handle error
            Debug.Log(err);
            Debug.LogError("PUT request error");
        });

        yield return new WaitUntil(() => response != null);
    }
    #endregion

    #region GET_request_functions
    private IEnumerator GetUpdatedUserData(string endpoint, bool IsAuthenticated = true)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        authenticatedUser = null;
        string url = BaseURL + endpoint;

        OnAuthStarted?.Invoke();
        RestClient.Get<StrapiUser>(url).Then(userResponse =>
        {
            authenticatedUser = userResponse;
            Debug.Log("User updated");
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => authenticatedUser != null);
    }

    private IEnumerator GetUsersRequest(string endpoint)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        users = null;
        string url = BaseURL + endpoint;

        OnAuthStarted?.Invoke();
        RestClient.GetArray<StrapiUser>(url).Then(userResponse =>
        {
            users = userResponse;
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => users != null);
    }

    private IEnumerator GetAuthRequest(string endpoint)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        AuthResponse response = null;
        OnAuthStarted?.Invoke();
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + userJWT;
        RestClient.Get<AuthResponse>(BaseURL + endpoint).Then(authResponse =>
        {
            response = authResponse;
            OnAuthSuccess?.Invoke(authResponse);
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => response != null);
    }

    private IEnumerator GetStrapiUserTeamsFromServer(string endpoint)
    {
        if (!IsAuthenticated)
        {
            Debug.LogWarning("User is not authenticated.");
            yield break;
        }

        teams = null;
        string url = BaseURL + endpoint;
        OnAuthStarted?.Invoke();
        RestClient.Get<StrapiUserTeamListResponse>(url).Then(response =>
        {
            teams = response.data;
            Debug.Log("Teams received successfully");
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });

        yield return new WaitUntil(() => teams != null);
    }

    private IEnumerator GetGroupDataRequest(string endpoint)
    {
        strapiUserTeam = null;
        string url = BaseURL + endpoint;

        OnAuthStarted?.Invoke();
        RestClient.Get<StrapiUserTeamResponse>(url).Then(response =>
        {
            strapiUserTeam = response.data.attributes;
            Debug.Log("Group data response successful");
        }).Catch(err =>
        {
            Debug.Log("GET request error");
        });

        yield return new WaitUntil(() => strapiUserTeam != null);
    }
    #endregion

    #region DELETE_request_functions
    private IEnumerator DeleteRequest(string endpoint)
    {
        string url = BaseURL + endpoint;
        Debug.Log(url);
        RestClient.Delete(BaseURL + endpoint).Then(deleteResponse =>
        {
            Debug.Log($"Successfully deleted data at {endpoint}");
        })
        .Catch(err =>
        {
            Debug.LogError($"Delete error request {err}");
        });

        yield return 0;
    }
    #endregion

    #region Getter_functions
    public String GetUsername()
    {
        return authenticatedUser.username;
    }

    public String GetEmail()
    {
        return authenticatedUser.email;
    }

    public String GetPassword()
    {
        return authenticatedUser.password;
    }

    public String GetName()
    {
        return authenticatedUser.firstname;
    }

    public String GetSurname()
    {
        return authenticatedUser.lastname;
    }

    public bool UserIsTeacher()
    {
        return userIsTeacher;
    }

    public StrapiUser[] GetUsers()
    {
        return users;
    }

    public StrapiTeamsData[] GetTeams()
    {
        return teams;
    }

    public StrapiUserTeam GetGroup()
    {
        return strapiUserTeam;
    }

    public StrapiUserTeam GetGroupByID(int id)
    {
        StrapiUserTeam team = null;

        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].id == id.ToString())
            {
                team = teams[i].attributes;
                break;
            }
        }

        return team;
    }
    #endregion

    private int GetUserTotalPoints(int userID)
    {
        return users[userID].firstgamescore + users[userID].secondgamescore + users[userID].thirdgamescore + users[userID].fourthgamescore + users[userID].fifthgamescore
            + users[userID].sixthgamescore + users[userID].seventhgamescore;
    }

    private int GetUserTotalPoints(StrapiUser user)
    {
        return user.firstgamescore + user.secondgamescore + user.thirdgamescore + user.fourthgamescore + user.fifthgamescore
            + user.sixthgamescore + user.seventhgamescore;
    }

    public StrapiUser GetCurrentUser()
    {
        return authenticatedUser;
    }
}
