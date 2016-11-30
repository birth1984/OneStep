using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    private static Player m_instance;

    public List<Hero> HeroList;

	private Player()
    {
        HeroList = new List<Hero>();
    }

    public static Player Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new Player();
            return m_instance;
        }
    }
}
