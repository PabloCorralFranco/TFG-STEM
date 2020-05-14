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
    private List<Arista> visited, notVisited;
    private string initialVertex;
    private Arista nextCorrectArista;
    private Graph graph;
    private int aristarActivadas;
    // Start is called before the first frame update
    void Start()
    {
        visited = new List<Arista>();
        notVisited = new List<Arista>();
        graph = FindObjectOfType<Graph>();
        aristarActivadas = 0;
    }

    public void setInitialVertex(string v1)
    {
        initialVertex = v1;
        calculateNextCorrectArista(initialVertex);
    }

    private void calculateNextCorrectArista(string v)
    {
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
            if(a._3() < nextCorrectArista._3())
            {
                nextCorrectArista = a;
            }
        }
        Debug.Log("Tienes que elegir la arista: " + nextCorrectArista.toString());
    }

    public void tryAristaActivation(Arista a)
    {
        if(a._1().Equals(nextCorrectArista._1()) && a._2().Equals(nextCorrectArista._2()) && a._3() == nextCorrectArista._3())
        {
            Debug.Log("Hemos accedido a la siguiente arista correcta" + a.toString());
            visitArista(a);
            //Debug.Log("Visitados: " + visited.ToString());
            //Debug.Log("No Visitados: " + notVisited.ToString());
            aristarActivadas += 1;
            if (checkEnd()) return;
            calculateNextCorrectArista(a._1());
            calculateNextCorrectArista(a._2());
        }
        else
        {
            Debug.Log("Se reinicia el puzzle");
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
        if (aristarActivadas == 6)
        {
            Debug.Log("Hemos Ganado!");
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




}
