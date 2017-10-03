using UnityEngine;
using System.Collections;

public class PlanetData
{
    public PlanetType data { get; private set; }

    public PlanetData(PlanetTypeEnum type)
    {
        switch (type)
        {
            case PlanetTypeEnum.Desert:
                data = new PlanetTypeEgypt();
            break;

            case PlanetTypeEnum.Forest:
                data = new PlanetTypeForest();
            break;

            case PlanetTypeEnum.Water:
                data = new PlanetTypeHawaii();
            break;

            case PlanetTypeEnum.Icy:
                data = new PlanetTypeIce();
            break;

            case PlanetTypeEnum.Plain:
                data = new PlanetTypeIceGrey();
            break;

            case PlanetTypeEnum.Clay:
                data = new PlanetTypeOrange();
            break;

            case PlanetTypeEnum.Tundra:
                data = new PlanetTypePine();
            break;
        }
    }
}

public class PlanetType
{
    public int numOfTries { get; protected set; }
    public int numOfDiamonds { get; protected set; }
    public int numOfGolds { get; protected set; }
    public int numOfSilvers { get; protected set; }
    public int numOfBronzes { get; protected set; }

    public PlanetType()
    {
        numOfTries = 6;
        numOfDiamonds = 3;
        numOfGolds = 3;
        numOfSilvers = 3;
        numOfBronzes = 3;
    }
}

public class PlanetTypeEgypt : PlanetType
{
    public PlanetTypeEgypt()
    {
        numOfTries = 6;
        numOfDiamonds = 0;
        numOfGolds = 3;
        numOfSilvers = 3;
        numOfBronzes = 6;
    }
}

public class PlanetTypeHawaii : PlanetType
{
    public PlanetTypeHawaii()
    {
        numOfTries = 5;
        numOfDiamonds = 1;
        numOfGolds = 2;
        numOfSilvers = 4;
        numOfBronzes = 5;
    }
}

public class PlanetTypeForest : PlanetType
{
    public PlanetTypeForest()
    {
        numOfTries = 4;
        numOfDiamonds = 2;
        numOfGolds = 4;
        numOfSilvers = 5;
        numOfBronzes = 6;
    }
}

public class PlanetTypeIce : PlanetType
{
    public PlanetTypeIce()
    {
        numOfTries = 2;
        numOfDiamonds = 5;
        numOfGolds = 5;
        numOfSilvers = 2;
        numOfBronzes = 2;
    }
}

public class PlanetTypeIceGrey : PlanetType
{
    public PlanetTypeIceGrey()
    {
        numOfTries = 3;
        numOfDiamonds = 2;
        numOfGolds = 5;
        numOfSilvers = 3;
        numOfBronzes = 2;
    }
}

public class PlanetTypeOrange : PlanetType
{
    public PlanetTypeOrange()
    {
        numOfTries = 4;
        numOfDiamonds = 1;
        numOfGolds = 3;
        numOfSilvers = 4;
        numOfBronzes = 4;
    }
}

public class PlanetTypePine : PlanetType
{
    public PlanetTypePine()
    {
        numOfTries = 3;
        numOfDiamonds = 3;
        numOfGolds = 3;
        numOfSilvers = 3;
        numOfBronzes = 3;
    }
}

