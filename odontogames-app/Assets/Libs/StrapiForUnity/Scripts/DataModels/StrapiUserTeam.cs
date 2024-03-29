using System;
using UnityEngine;

namespace StrapiForUnity
{
    [System.Serializable]
    public class StrapiMember
    {
        public int id;
        public StrapiUser attributes;
    }

    [System.Serializable]
    public class StrapiTeamMember
    {
        public StrapiMember[] data;
    }

    [System.Serializable]
    public class StrapiUserTeam
    {
        public int id;
        public string teamname;
        public int teamscore;
        public int numplayers;
        public string creator;

        [SerializeField]
        private string createdAt;
        private DateTime? _createdAt;
        public DateTime? CreatedAt()
        {
            if (_createdAt == null)
            {
                _createdAt = Convert.ToDateTime(createdAt);
            }
            return _createdAt;
        }

        [SerializeField]
        private string updatedAt;
        private DateTime? _updatedAt;
        public DateTime? UpdatedAt()
        {
            if (_updatedAt == null)
            {
                _updatedAt = Convert.ToDateTime(updatedAt);
            }
            return _updatedAt;
        }

        public StrapiTeamMember members;
    }

    [System.Serializable]
    public class StrapiTeamsData
    {
        public string id;
        public StrapiUserTeam attributes;
    }

    [Serializable]
    public class StrapiUserTeamResponse
    {
        public StrapiTeamsData data;
    }

    [Serializable]
    public class StrapiUserTeamListResponse
    {
        public StrapiTeamsData[] data;
    }
}
