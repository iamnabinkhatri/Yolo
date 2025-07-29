using System;

namespace YoloSoccerApp.Logic
{
	public class UserRole
	{

		//property
		public int _id { get; set; }
		public string? _roleType { get; set; }

		public UserRole() { }

        public UserRole(int id, string roleType)
        {
			this._id = id;
            this._roleType = roleType;
        }
        public UserRole(string roleType)
		{
			this._roleType = roleType;
		}
        //methods
        public override string ToString()
        {
            return @$"\nUserRole ID: {this._id} \n RoleType: {this._roleType}";
        }
    }
}

