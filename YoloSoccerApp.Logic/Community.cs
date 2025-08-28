namespace YoloSoccerApp.Logic;

public class Community
{
    public int _id { get; set; }
    public string _communityName { get; set; }
    public string _description { get; set; }
    
    public Community() { }

    public Community(int id)
    {
        _id = id;
    }

    public Community(int id, string communityName, string description)
    {
        _id = id;
        _communityName = communityName;
        _description = description;
    }

    public override string ToString()
    {
        return $@"
        id: {this._id}
        CommunityName: {this._communityName}
        Description: {this._description}
        ";
    }
}
