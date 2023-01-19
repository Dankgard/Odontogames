﻿﻿using System;
using System.Collections;
using LitJson;
using StrapiForUnity;
using UnityEngine;
using Proyecto26;

public class StrapiComponent : MonoBehaviour
{
    public event Action<AuthResponse> OnAuthSuccess = delegate {  };
    public event Action<Exception> OnAuthFail = delegate {  };

    public event Action OnAuthStarted = delegate {  };
    public event Action NoStoredJWT = delegate { };

    public event Action deleteAccount = delegate { };
    
    [Tooltip("The root URL of your Strapi server. For example: http://localhost:1337")]
    public string BaseURL;
    
    [Tooltip("The secret key used for creating the JWT on your Strapi server. This is used to check that the stored JWT is valid. The secret can be found in your Strapi installation, at 'strapi/extensions/users-permissions/config/jwt.js'")]
    public string JWTSecret;
    
    public StrapiUser AuthenticatedUser;
    public bool IsAuthenticated = false;
    public string ErrorMessage;

    private bool rememberMe = true;

    private string userJWT = "";
    private string userID = "";

    private StrapiUser[] users = null;
    private StrapiGroup[] groups = null;

    //Singleton
    public static StrapiComponent _instance;
    
    protected virtual void Awake()
    {
        if (!BaseURL.EndsWith("/"))
        {
            BaseURL += "/";
        }

        //Singleton
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
    
    // Start is called before the first frame update
    public void Start()
    {
        //checkForSavedJWT();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAuthSuccessEvent(AuthResponse authResponse)
    {
        OnAuthSuccess.Invoke(authResponse);
    }

    public void TriggerAuthFailEvent(Exception exception)
    {
        OnAuthFail.Invoke(exception);
    }

    public void TriggerOnAuthStartedEvent()
    {
        OnAuthStarted.Invoke();
    }

    private void checkForSavedJWT()
    {
        //string jwt = PlayerPrefs.GetString("jwt", "");
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
        if (JWTSecret=="")
        {
            Debug.Log("Couldn't validate stored JWT. You should consider setting your Strapi Component JWT secret.");
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
                Debug.Log("The token has expired. Deleting.");
                PlayerPrefs.DeleteKey("jwt");
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
    
    public void Login(string username, string password, bool rememberMe = false) {
        OnAuthStarted?.Invoke();
        this.rememberMe = rememberMe;
        
        var jsonString = "{" +
                         "\"identifier\":\"" + username + "\"," +
                         "\"password\":\"" + password + "\"" +
                         "}";

        PostAuthRequest("api/auth/local", jsonString);
    }

    public void Register(string username, string name, string surname, string email, string password, bool rememberMe = false/*, Dictionary<string,string> extraAttributes = null*/) {
        OnAuthStarted?.Invoke();
        this.rememberMe = rememberMe;

        var jsonString = "{" +
                         "\"username\":\"" + username + "\"," +
                         "\"name\":\"" + name + "\"," +
                         "\"surname\":\"" + surname + "\"," +
                         "\"email\":\"" + email + "\"," +
                         "\"password\":\"" + password + "\"";

        //if (extraAttributes != null && extraAttributes.Count > 0)
        //{
        //    foreach (var attribute in extraAttributes)
        //    {
        //        jsonString += "\""+attribute.Key+"\":\"" + attribute.Value + "\",";
        //    }
        //    jsonString = jsonString.Remove(jsonString.Length - 1); // removes the final trailing comma
        //}
        jsonString += "}";

        PostAuthRequest("api/auth/local/register", jsonString);
    }

    public virtual void DeleteAccount() {
        OnAuthStarted?.Invoke();
        string endpoint = "api/users/" + userID;
    }

    public virtual void EditProfile(string username)
    {
        if (username != AuthenticatedUser.username)
        {
            OnAuthStarted?.Invoke();
            var jsonString = "{" +
                         "\"username\":\"" + username + "\"," +
                         "\"email\":\"" + AuthenticatedUser.email + "\"," +
                         "\"password\":\"" + "new password" + "\"";
            jsonString += "}";

            string endpoint = "api/users/" + AuthenticatedUser.id.ToString();
            PutRequest(endpoint, jsonString, userJWT);
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
        var jsonString = "{" +
                         "\"username\":\"" + user.username + "\"," +
                         "\"email\":\"" + user.email + "\"," +
                         "\"Group\":\"" + group + "\"";
        jsonString += "}";

        string endpoint = "api/users/" + user.id;

        PutRequest(endpoint, jsonString, userJWT);
    }

    public virtual void PutRequest(string endpoint, string jsonString, string jwt)
    {
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + userJWT;
        RestClient.Put<AuthResponse>(BaseURL + endpoint, jsonString).Then(authResponse =>
        {
            OnAuthSuccess?.Invoke(authResponse);
        }).Catch(err =>
        {
            OnAuthFail?.Invoke(err);
            Debug.Log($"Authentication Error: {err}");
        });
    }

    public virtual void DeleteRequest(string endpoint)
    {
        RestClient.Delete(BaseURL + endpoint);
        userID = "";
    }

    public virtual void PostAuthRequest(string endpoint, string jsonString)
    {
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

    public virtual void GetAuthRequest(string endpoint)
    {
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

    /// <summary>
    /// Login with an authentication token
    /// </summary>
    /// <param name="jwt"></param>
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

    public void OnAuthSuccessHandler(AuthResponse authResponse)
    {
        AuthenticatedUser = authResponse.user;
        userJWT = authResponse.jwt;
        userID = authResponse.user.id.ToString();
        IsAuthenticated = true;
        
        RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + userJWT;
        Debug.Log($"Successfully authenticated. Welcome {AuthenticatedUser.username}");
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
