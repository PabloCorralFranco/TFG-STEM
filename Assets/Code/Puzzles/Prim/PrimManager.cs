using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimManager : MonoBehaviour
{

    /// <summary>
    /// Necesitamos una lista de aristas abiertas.
    /// Y otra lista de aristas exploradas.
    /// Entonces cuando activemos una nueva arista debemos comprobar si pertenece a la lista de aristas abiertas y a su vez si es la de peso minimo
    /// Ejemplo: Activamos A como inicio del arbol minimal - Tenemos dos aristas (a,d,5) y (a,b,7), entonces ahora el jugador activa una arista (la que sea, puede ser hasta alguna no relacionada)
    /// El cable nos manda la arista que representa y comprobamos si esta arista es la solucion esperada (que seria (a,d,5)), si es asi activamos un efecto entre a-d, si no reiniciamos todo.
    /// Y ahora añadimos a la lista de aristas por explorar las colindantes con d, que serian (d,b,9), (d,e,15), (d,f,6) y seguiriamos el ciclo.
    /// Cuando el jugador crea que ha terminado le daria a un boton para comprobar la solucion. Si acierta sale del bosque, y si no es transportado a una zona de enemigos que debe derrotar para poder salir.
    /// Tenemos que tener en cuenta que no se produzcan ciclos, es un arbol no un grafo.
    /// </summary>
    public GameObject portal;
    private List<Arista> visited, notVisited;
    private List<string> conjuntoVertices;
    private List<string> connectedPorts;
    private string initialVertex;
    private Arista nextCorrectArista;
    private Graph graph;
    private int aristasActivadas;
    private GameObject cableToActive;
    private EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        visited = new List<Arista>();
        notVisited = new List<Arista>();
        conjuntoVertices = new List<string>();
        connectedPorts = new List<string>();
        graph = FindObjectOfType<Graph>();
        aristasActivadas = 0;
        eventManager = FindObjectOfType<EventManager>();
    }

    public void setInitialVertex(string v1)
    {
        initialVertex = v1;
        calculateNextCorrectArista(initialVertex);
    }

    private void calculateNextCorrectArista(string v)
    {
        //Añadimos el vertice que nos ayudara a comprobar si existen ciclos.
        if (!conjuntoVertices.Contains(v))
        {
            conjuntoVertices.Add(v);
        }
        foreach(string s in conjuntoVertices)
        {
            Debug.Log("vertice explorado:" + s);
        }
        Arista[] aristas = graph.getAristas(v);
        //Introduciomos nuevas aristas a las no visitadas
        for(int i = 0; i < aristas.Length; i++)
        {
            //Si la arista no esta en visitados ni en no visitados
            if (!visitedContains(aristas[i]) && !notVisitedContains(aristas[i]))
            {
                notVisited.Add(aristas[i]);
                Debug.Log("Nueva arista: " + aristas[i].toString());
            }
        }
        //Una vez que disponemos de todas las aristas calculamos la solucion
        nextCorrectArista = notVisited[0];
        foreach(Arista a in notVisited)
        {
            if(a._3() <= nextCorrectArista._3() && (!conjuntoVertices.Contains(a._1()) || !conjuntoVertices.Contains(a._2()) ))
            {
                nextCorrectArista = a;
            }
        }
        Debug.Log("Tienes que elegir la arista: " + nextCorrectArista.toString());
    }

    public void tryAristaActivation(Arista a)
    {
        if(a._1().Equals(nextCorrectArista._1()) && a._2().Equals(nextCorrectArista._2()) && a._3() == nextCorrectArista._3() && connectedPorts.Contains(a._1()) && connectedPorts.Contains(a._2()))
        {
            Debug.Log("Hemos accedido a la siguiente arista correcta" + a.toString());

            changeEffectsState(a,false);
            visitArista(a);
            //Debug.Log("Visitados: " + visited.ToString());
            //Debug.Log("No Visitados: " + notVisited.ToString());
            aristasActivadas += 1;
            if (checkEnd()) return;
            calculateNextCorrectArista(a._1());
            calculateNextCorrectArista(a._2());
        }
        else
        {
            Debug.Log("Se reinicia el puzzle");
            visited.Clear();
            notVisited.Clear();
            conjuntoVertices.Clear();
            connectedPorts.Clear();
            initialVertex = null;
            aristasActivadas = 0;
            changeEffectsState(a,true);
        }
    }

    private void changeEffectsState(Arista a, bool canDeactivate)
    {
        Cable spriteToActive = cableToActive.GetComponent<Cable>();
        if (spriteToActive.arista._1().Equals(a._1()) && spriteToActive.arista._2().Equals(a._2()) && !canDeactivate)
        {
            spriteToActive.changeColliderState(false);
            for (int i = 0; i < spriteToActive.transform.childCount; i++)
            {
                spriteToActive.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Desactivamos los efectos");
            //Desactivamos todos los efectos porque nos hemos equivocado
            Vertex[] vert = FindObjectsOfType<Vertex>();
            for (int i = 0; i < vert.Length; i++)
            {
                vert[i].depush();
            }
            Cable[] allCables = FindObjectsOfType<Cable>();
            for(int i = 0; i < allCables.Length; i++)
            {
                allCables[i].changeColliderState(true);
                for (int j = 0; j < allCables[i].transform.childCount; j++)
                {
                    allCables[i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    public void visitArista(Arista a)
    {
        visited.Add(a);
        if (notVisitedContains(a))
        {
            removeNotVisited(a);
        }
    }

    private bool checkEnd()
    {
        if (aristasActivadas == 6)
        {
            Debug.Log("Hemos Ganado!");
            portal.SetActive(true);
            return true;
        }

        return false;
    }

    //Operaciones de ayuda para la iteracion y borrado de aristas en listas

    private bool visitedContains(Arista a)
    {
        bool contains = false;
        Arista[] varray = visited.ToArray();
        int i = 0;
        while (!contains && (i < varray.Length))
        {
            if (a._1().Equals(varray[i]._1()) && a._2().Equals(varray[i]._2()) && a._3() == varray[i]._3())
            {
                contains = true;
            }
            i++;
        }
        return contains;
    }

    private bool notVisitedContains(Arista a)
    {
        bool contains = false;
        Arista[] varray = notVisited.ToArray();
        int i = 0;
        while (!contains && (i < varray.Length))
        {
            if (a._1().Equals(varray[i]._1()) && a._2().Equals(varray[i]._2()) && a._3() == varray[i]._3())
            {
                contains = true;
            }
            i++;
        }
        return contains;
    }

    private bool removeNotVisited(Arista a)
    {
        bool contains = false;
        Arista[] varray = notVisited.ToArray();
        Arista toRemove = null;
        int i = 0;
        while (!contains && (i < varray.Length))
        {
            if (a._1().Equals(varray[i]._1()) && a._2().Equals(varray[i]._2()) && a._3() == varray[i]._3())
            {
                contains = true;
                toRemove = varray[i];
            }
            i++;
        }
        notVisited.Remove(toRemove);
        return contains;
    }

    public void connectedPort(string vertexName, bool isConnected)
    {
        if (isConnected)
        {
            connectedPorts.Add(vertexName);
        }
        else
        {
            connectedPorts.Remove(vertexName);
        }
    }

    public bool isInitialVertexNull()
    {
        return (initialVertex == null);
    }

    public void setCableToActive(GameObject cable)
    {
        cableToActive = cable;
    }
}
