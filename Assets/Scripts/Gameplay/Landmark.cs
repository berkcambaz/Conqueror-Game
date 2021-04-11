public class Landmark
{
    public LandmarkID id;

    public Landmark(LandmarkID _id)
    {
        id = _id;
    }

    public int GetBonus(bool againstArmy)
    {
        switch (id)
        {
            case LandmarkID.Capital:
                return 0;
            case LandmarkID.Church:
                return 0;
            case LandmarkID.Forest:
                if (againstArmy)
                    return 1;
                return 0;
            case LandmarkID.House:
                return 0;
            case LandmarkID.Mountains:
                return 0;
            case LandmarkID.Tower:
                return 0;
            default:
                return 0;
        }
    }
    
    public int GetPenalty(bool againstArmy)
    {

        switch (id)
        {
            case LandmarkID.Capital:
                if (!againstArmy)
                    return -2;
                return 0;
            case LandmarkID.Church:
                if (!againstArmy)
                    return 2;
                return 0;
            case LandmarkID.Forest:
                return -1;
            case LandmarkID.House:
                if (!againstArmy)
                    return 1;
                return 0;
            case LandmarkID.Mountains:
                return -1;
            case LandmarkID.Tower:
                return -1;
            default:
                return 0;
        }
    }
}

public enum LandmarkID
{
    None = -1,
    Capital,
    Church,
    Forest,
    House,
    Mountains,
    Tower,
    Count
}