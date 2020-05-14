using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Dictionary<string, Arista[]> graphDict;
    private string[] vertices;
    private Arista[][] aristas;
    private Arista ad,ab,db,de,df,be,bc,ec,eg,fe,fg;
    private Arista[] a, b, c, d, e, f, g;
    void Start()
    {
        graphDict = new Dictionary<string, Arista[]>();
        ad = new Arista("a", "d", 5);
        ab = new Arista("a", "b", 7);
        db = new Arista("d", "b", 9);
        de = new Arista("d", "e", 15);
        df = new Arista("d", "f", 6);
        be = new Arista("b", "e", 7);
        bc = new Arista("b", "c", 8);
        ec = new Arista("e", "c", 5);
        eg = new Arista("e", "g", 9);
        fe = new Arista("f", "e", 8);
        fg = new Arista("f", "g", 11);
        a = new Arista[] { ad, ab };
        d = new Arista[] { ad, db, de, df };
        b = new Arista[] { ab, db, be, bc};
        f = new Arista[] { df, fe, fg};
        c = new Arista[] { bc, ec};
        e = new Arista[] { be, de, fe, ec, eg};
        g = new Arista[] { fg, eg};
        aristas = new Arista[][] { a,b,c,d,e,f,g };
        vertices = new string[] { "a", "b", "c", "d", "e", "f", "g" };
        //Tiene que haber tantos vertices como aristas
        for (int i = 0; i < vertices.Length; i++)
        {
            graphDict.Add(vertices[i], aristas[i]);
        }
    }

    public Arista[] getAristas(string vertexName)
    {
        if (graphDict.ContainsKey(vertexName))
        {
            Arista[] temp;
            graphDict.TryGetValue(vertexName,out temp);
            return temp;
        }
        return null;
    }
}
