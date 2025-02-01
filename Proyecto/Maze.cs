using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Media;

public class Cell : IComparable<Cell>
{
    public int x { get; }
    public int y { get; }
    public int d { get; }
    public string val { get; }

    public Cell(int _x, int _y, string _v, int _d)
    {
        x = _x;
        y = _y;
        val = _v;
        d = _d;
    }

    public int CompareTo(Cell? b)
    {
        if (b == null) return 1;
        return this.d.CompareTo(b.d);
    }
}
public class Grid
{
    int rows { get; }
    int col { get; }
    private string[,] grid;
    private bool[,] visited;

    private string p1, p2;
    private int x1 = -1, y1 = -1, x2 = -1, y2 = -1, size = 0, hp1, hp2, e1, e2, total_hp1, total_hp2, total_e1, total_e2;
    private int start_x1, start_x2, start_y1, start_y2, desmayo1 = 0, desmayo2 = 0, steps1, steps2, recoil1, recoil2, obs;
    private bool hab1=false, hab2=false;

    List <(int, int)> trees;

    //Constructor
    public Grid(int n, int m, string s1, string s2, bool mp, int t)
    {
        rows = (n % 2 == 0 ? n + 1 : n + 2);
        col = (m % 2 == 0 ? m + 1 : m + 2);
        grid = new string[rows, col];
        visited = new bool[rows, col];
        p1 = s1;
        p2 = s2;

        if (s1 == "🧌") { total_hp1 = hp1 = 100; total_e1 = e1 = 80; steps1=1;}
        if (s1 == "🧟") { total_hp1 = hp1 = 80; total_e1 = e1 = 50; steps1=2;}
        if (s1 == "🧛") { total_hp1 = hp1 = 70; total_e1 = e1 = 60; steps1=2;}
        if (s1 == "🧝") { total_hp1 = hp1 = 70; total_e1 = e1 = 70; steps1=3;}
        if (s1 == "🧙") { total_hp1 = hp1 = 50; total_e1 = e1 = 90; steps1=3;}

        if (s2 == "🧌") { total_hp2 = hp2 = 100; total_e2 = e2 = 80; steps2=1;}
        if (s2 == "🧟") { total_hp2 = hp2 = 80; total_e2 = e2 = 50; steps2=2;}
        if (s2 == "🧛") { total_hp2 = hp2 = 70; total_e2 = e2 = 60; steps2=2;}
        if (s2 == "🧝") { total_hp2 = hp2 = 70; total_e2 = e2 = 70; steps2=3;}
        if (s2 == "🧙") { total_hp2 = hp2 = 50; total_e2 = e2 = 90; steps2=3;}

        size = t;
        if(t==1)obs=3;
        if(t==2)obs=5;
        if(t==3)obs=20;

        trees = new List<(int, int)>();
        Init();
        GenerateMaze(1, 1);
        FindStartPosition();
        start_x1 = x1; start_y1 = y1;
        start_x2 = x2; start_y2 = y2;
        Obstaculos();
        Jugar(mp);
    }

    public void Init()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < col; j++)
            {
                // Initialize visited array
                visited[i, j] = false;
                if (i == rows - 2 && j == col - 2) grid[i, j] = "⬛";
                else if (i == 0 || i == rows - 1 || j == 0 || j == col - 1) grid[i, j] = "⬜";
                else if (i % 2 == 1 && j % 2 == 1) grid[i, j] = "⬛";
                else grid[i, j] = "⬜";
            }
        }
    }

    public bool ValidCell(int r, int c)
    {
        return r > 0 && c > 0 && r < rows - 1 && c < col - 1;
    }

    public void GenerateMaze(int r, int c)
    {
        visited[r, c] = true;

        List<Cell> adj = new List<Cell>();
        bool flag = false;

        //Add non-visited neighbours to the list
        if (ValidCell(r + 2, c) && visited[r + 2, c] == false)
        {
            Cell next = new Cell(r + 2, c, grid[r + 2, c], 0);
            adj.Add(next);
            flag = true;
        }

        if (ValidCell(r - 2, c) && visited[r - 2, c] == false)
        {
            Cell next = new Cell(r - 2, c, grid[r - 2, c], 0);
            adj.Add(next);
            flag = true;
        }

        if (ValidCell(r, c + 2) && visited[r, c + 2] == false)
        {
            Cell next = new Cell(r, c + 2, grid[r, c + 2], 0);
            adj.Add(next);
            flag = true;
        }

        if (ValidCell(r, c - 2) && visited[r, c - 2] == false)
        {
            Cell next = new Cell(r, c - 2, grid[r, c - 2], 0);
            adj.Add(next);
            flag = true;
        }

        //There is no valid neighbour
        if (!flag) return;

        Random rand = new Random();
        while (adj.Count > 0)
        {
            int idx = rand.Next(adj.Count);
            Cell next = adj[idx];

            if (visited[next.x, next.y] == true)
            {
                adj.Remove(next);
                continue;
            }

            if (next.x == r + 2) grid[r + 1, c] = "⬛";
            if (next.x == r - 2) grid[r - 1, c] = "⬛";
            if (next.y == c + 2) grid[r, c + 1] = "⬛";
            if (next.y == c - 2) grid[r, c - 1] = "⬛";

            //dfs
            GenerateMaze(next.x, next.y);
            adj.Remove(next);
        }
    }

    public int[,] BFS(int r, int c)
    {
        Queue<Cell> q = new Queue<Cell>();
        int[,] dist = new int[rows + 1, col + 1];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < col; j++)
            {
                dist[i, j] = int.MaxValue;
            }
        }

        Cell start = new Cell(r, c, "⬛", 0);
        dist[r, c] = 0;
        q.Enqueue(start);

        while (q.Count() > 0)
        {
            Cell curr = q.Dequeue();

            if (ValidCell(curr.x + 1, curr.y) && grid[curr.x + 1, curr.y] == "⬛" && dist[curr.x + 1, curr.y] > dist[curr.x, curr.y] + 1)
            {
                dist[curr.x + 1, curr.y] = dist[curr.x, curr.y] + 1;
                Cell next = new Cell(curr.x + 1, curr.y, "⬛", 0);
                q.Enqueue(next);
            }

            if (ValidCell(curr.x - 1, curr.y) && grid[curr.x - 1, curr.y] == "⬛" && dist[curr.x - 1, curr.y] > dist[curr.x, curr.y] + 1)
            {
                dist[curr.x - 1, curr.y] = dist[curr.x, curr.y] + 1;
                Cell next = new Cell(curr.x - 1, curr.y, "⬛", 0);
                q.Enqueue(next);
            }

            if (ValidCell(curr.x, curr.y + 1) && grid[curr.x, curr.y + 1] == "⬛" && dist[curr.x, curr.y + 1] > dist[curr.x, curr.y] + 1)
            {
                dist[curr.x, curr.y + 1] = dist[curr.x, curr.y] + 1;
                Cell next = new Cell(curr.x, curr.y + 1, "⬛", 0);
                q.Enqueue(next);
            }

            if (ValidCell(curr.x, curr.y - 1) && grid[curr.x, curr.y - 1] == "⬛" && dist[curr.x, curr.y - 1] > dist[curr.x, curr.y] + 1)
            {
                dist[curr.x, curr.y - 1] = dist[curr.x, curr.y] + 1;
                Cell next = new Cell(curr.x, curr.y - 1, "⬛", 0);
                q.Enqueue(next);
            }
        }

        return dist;
    }

    public void FindStartPosition()
    {

        int[,] dist1 = BFS(rows - 2, col - 2);

        List<Cell> cells = new List<Cell>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grid[i, j] != "⬛") continue;
                Cell c = new Cell(i, j, "⬛", dist1[i, j]);
                cells.Add(c);
            }
        }

        cells.Sort();

        Cell first = cells[cells.Count() - 1];
        cells.Remove(first);
        x1 = first.x; y1 = first.y;

        Queue<Cell> q = new Queue<Cell>();
        q.Enqueue(first);

        bool[,] mark = new bool[rows, col];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < col; j++)
            {
                mark[i, j] = false;
            }
        }

        mark[first.x, first.y] = true;
        while (q.Count() > 0)
        {
            Cell curr = q.Dequeue();
            if (curr.x == rows - 2 && curr.y == col - 2) break;

            int new_x = 0, new_y = 0, mi = int.MaxValue;

            if (ValidCell(curr.x + 1, curr.y) && grid[curr.x + 1, curr.y] == "⬛")
            {
                if (dist1[curr.x + 1, curr.y] < mi)
                {
                    new_x = curr.x + 1;
                    new_y = curr.y;
                    mi = dist1[new_x, new_y];
                }
            }

            if (ValidCell(curr.x - 1, curr.y) && grid[curr.x - 1, curr.y] == "⬛")
            {
                if (dist1[curr.x - 1, curr.y] < mi)
                {
                    new_x = curr.x - 1;
                    new_y = curr.y;
                    mi = dist1[new_x, new_y];
                }
            }

            if (ValidCell(curr.x, curr.y + 1) && grid[curr.x, curr.y + 1] == "⬛")
            {
                if (dist1[curr.x, curr.y + 1] < mi)
                {
                    new_x = curr.x;
                    new_y = curr.y + 1;
                    mi = dist1[new_x, new_y];
                }
            }

            if (ValidCell(curr.x, curr.y - 1) && grid[curr.x, curr.y - 1] == "⬛")
            {
                if (dist1[curr.x, curr.y - 1] < mi)
                {
                    new_x = curr.x;
                    new_y = curr.y - 1;
                    mi = dist1[new_x, new_y];
                }
            }

            mark[new_x, new_y] = true;
            Cell next = new Cell(new_x, new_y, "⬛", 0);
            q.Enqueue(next);
        }

        int ma = 0;
        foreach (Cell c in cells)
        {
            if (mark[c.x, c.y] == true) continue;
            if (dist1[c.x, c.y] > ma)
            {
                ma = dist1[c.x, c.y];
                x2 = c.x;
                y2 = c.y;
            }
        }

        if (x2 == -1)
        {
            Init();
            GenerateMaze(rows, col);
            FindStartPosition();
        }

    }

    public void Obstaculos()
    {
        List<Cell> cells = new List<Cell>();

        grid[rows - 2, col - 2] = "🚩";

        for (int i = 1; i < rows - 1; i++)
        {
            for (int j = 1; j < col - 1; j++)
            {
                if (i == x1 && j == y1) continue;
                if (i == x2 && j == y2) continue;
                if (i == rows - 2 && j == col - 2) continue;
                if (grid[i, j] == "⬜") continue;
                Cell c = new Cell(i, j, grid[i, j], 0);
                cells.Add(c);
            }
        }

            for (int i = 0; i < obs; i++)
            {
                Random rand = new Random();
                int idx = rand.Next(cells.Count);
                Cell c = cells[idx];
                cells.Remove(c);
                grid[c.x, c.y] = "🌳";
                trees.Add((c.x, c.y));
            }

            for (int i = 0; i < obs; i++)
            {
                Random rand = new Random();
                int idx = rand.Next(cells.Count);
                Cell c = cells[idx];
                cells.Remove(c);
                grid[c.x, c.y] = "🗻";
            }

            for (int i = 0; i < obs; i++)
            {
                Random rand = new Random();
                int idx = rand.Next(cells.Count);
                Cell c = cells[idx];
                cells.Remove(c);
                grid[c.x, c.y] = "💣";
            }

            for (int i = 0; i < obs; i++)
            {
                Random rand = new Random();
                int idx = rand.Next(cells.Count);
                Cell c = cells[idx];
                cells.Remove(c);
                grid[c.x, c.y] = "👹";
            }
    }

    public void Jugar(bool mp)
    {
        bool turn = true;

        while (true)
        {

             if(turn && recoil1>0){
                    recoil1--;
                }

            if(!turn && recoil2>0){
                    recoil2--;
                }

            for(int i=0; i<(turn ? steps1 : steps2); i++){

                 if ((x1 == rows - 2 && y1 == col - 2) || (x2 == rows - 2 && y2 == col - 2)) break;

            Console.Clear();
            PrintMaze();
            Console.WriteLine($"Jugador 1: ❤️  Vida - {hp1}/{total_hp1}   ⚡​ Energia - {e1}/{total_e1}");


            if (mp)
            {
                int p = turn ? 1 : 2;

                Console.WriteLine($"Jugador 2: ❤️  Vida - {hp2}/{total_hp2}   ⚡​ Energia - {e2}/{total_e2}");
                Console.WriteLine($"Turno del jugador {p}");

                if (p == 1 && desmayo1 > 0)
                {
                    desmayo1--;
                    Console.WriteLine($"El jugador 1 esta desmayado ({desmayo1} turnos restantes)");
                    Console.ReadKey();
                    if (desmayo1 == 0)
                    {
                        hp1 = total_hp1;
                        e1 = total_e1;
                    }
                    break;
                }

                if (p == 2 && desmayo2 > 0)
                {
                    desmayo2--;
                    Console.WriteLine($"El jugador 2 esta desmayado ({desmayo2} turnos restantes)");
                    Console.ReadKey();
                    if (desmayo2 == 0)
                    {
                        hp2 = total_hp2;
                        e2 = total_e2;
                    }
                    break;
                }

                bool flag = false;
                if (p == 1 && ValidCell(x1 + 1, y1) && grid[x1 + 1, y1] != "⬜" && (x1 + 1 != x2 || y1 != y2)) flag = true;
                if (p == 2 && ValidCell(x2 + 1, y2) && grid[x2 + 1, y2] != "⬜" && (x2 + 1 != x1 || y1 != y2)) flag = true;
                if (p == 1 && ValidCell(x1 - 1, y1) && grid[x1 - 1, y1] != "⬜" && (x1 - 1 != x2 || y1 != y2)) flag = true;
                if (p == 2 && ValidCell(x2 - 1, y2) && grid[x2 - 1, y2] != "⬜" && (x2 - 1 != x1 || y1 != y2)) flag = true;
                if (p == 1 && ValidCell(x1, y1 + 1) && grid[x1, y1 + 1] != "⬜" && (x1 != x2 || y1 + 1 != y2)) flag = true;
                if (p == 2 && ValidCell(x2, y2 + 1) && grid[x2, y2 + 1] != "⬜" && (x2 != x1 || y1 != y2 + 1)) flag = true;
                if (p == 1 && ValidCell(x1, y1 - 1) && grid[x1, y1 - 1] != "⬜" && (x1 != x2 || y1 - 1 != y2)) flag = true;
                if (p == 2 && ValidCell(x2, y2 - 1) && grid[x2, y2 - 1] != "⬜" && (x2 != x1 || y1 != y2 - 1)) flag = true;

                if (flag == false)
                {
                    Console.WriteLine($"El jugador {p} no tiene movimientos validos");
                    Console.ReadKey();
                    break;
                }

            }

            else
            {
                if (desmayo1 > 0)
                {
                    desmayo1--;
                    Console.WriteLine($"El jugador 1 esta desmayado ({desmayo1} turnos restantes)");
                    Console.ReadKey();
                    if (desmayo1 == 0)
                    {
                        hp1 = total_hp1;
                        e1 = total_e1;
                    }
                    break;
                }
            }

            Console.WriteLine("Presione E para activar su habilidad");

            ConsoleKeyInfo mov = Console.ReadKey();
            string? s = mov.Key.ToString();

            int new_x=0, new_y=0;

            if (s == "W")
            {
                if (turn)
                {
                    new_x = x1 - 1;
                    new_y = y1;
                }
                else
                {
                    new_x = x2 - 1;
                    new_y = y2;
                }
            }

            else if (s == "S")
            {
                if (turn)
                {
                    new_x = x1 + 1;
                    new_y = y1;
                }
                else
                {
                    new_x = x2 + 1;
                    new_y = y2;
                }
            }

            else if (s == "A")
            {
                if (turn)
                {
                    new_x = x1;
                    new_y = y1 - 1;
                }
                else
                {
                    new_x = x2;
                    new_y = y2 - 1;
                }
            }

            else if (s == "D")
            {
                if (turn)
                {
                    new_x = x1;
                    new_y = y1 + 1;
                }
                else
                {
                    new_x = x2;
                    new_y = y2 + 1;
                }
            }

            else if(s=="E"){
                i--;
                if(turn && recoil1>0){
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine($"Debes esperar {recoil1} turnos para usar tu habilidad");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                if(!turn && recoil2>0){
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine($"Debes esperar {recoil2} turnos para usar tu habilidad");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                if(turn && p1=="🧌"){
                    Console.Clear();
                    Console.WriteLine("Has usado Golpe Devastador. en tu proximo movimiento destruiras la casilla a la que te muevas");
                    Console.ReadKey();
                    Console.Clear();
                    hab1=true;
                    recoil1=2;
                    continue;
                }

                if(!turn && p2=="🧌"){
                    Console.Clear();
                    Console.WriteLine("Has usado Golpe Devastador. en tu proximo movimiento destruiras la casilla a la que te muevas");
                    Console.ReadKey();
                    Console.Clear();
                    hab2=true;
                    recoil2=2;
                    continue;
                }

                if(turn && p1=="🧟"){
                    Console.Clear();
                    Console.WriteLine("Has usado Mordida de Zombie. en tu proximo movimiento tienes la posibilidad de morder al otro jugador y asesinarlo");
                    Console.ReadKey();
                    Console.Clear();
                    hab1=true;
                    recoil1=8;
                    continue;
                }

                if(!turn && p2=="🧟"){
                    Console.Clear();
                    Console.WriteLine("Has usado Mordida de Zombie. en tu proximo movimiento tienes la posibilidad de morder al otro jugador y asesinarlo");
                    Console.ReadKey();
                    Console.Clear();
                    hab2=true;
                    recoil2=8;
                    continue;
                }

                if(turn && p1=="🧛"){
                    Console.Clear();
                    Console.WriteLine("Has usado Absorcion de Sangre. Jugador 2: ❤️ -10   Jugador 1: ❤️ +10");
                    hp1=Math.Min(total_hp1, hp1+10);
                    hp2-=10;

                     if (hp2 <= 0)
                {
                    x2 = start_x2;
                    y2 = start_y2;
                    Console.Clear();
                    Console.WriteLine("El jugador 2 ha muerto.");
                    hp2 = total_hp2;
                    e2 = total_e2;
                }
                    
                    Console.ReadKey();
                    Console.Clear();
                    recoil1=3;
                    continue;
                }

                if(!turn && p2=="🧛"){
                    Console.Clear();
                    Console.WriteLine("Has usado Absorcion de Sangre. Jugador 1: ❤️ -10   Jugador 2: ❤️ +10");
                    hp2=Math.Min(total_hp2, hp2+10);
                    hp1-=10;

                     if (hp1 <= 0)
                {
                    x1 = start_x1;
                    y1 = start_y1;
                    Console.Clear();
                    Console.WriteLine("El jugador 1 ha muerto.");
                    hp1 = total_hp1;
                    e1 = total_e1;
                }
                    
                    Console.ReadKey();
                    Console.Clear();
                    recoil2=3;
                    continue;
                }

                if(turn && p1=="🧙"){
                    Console.Clear();
                    Console.WriteLine("Has usado Rayo Paralizador. Te saltaras el proximo turno del jugador 2");
                    Console.ReadKey();
                    i-=steps1-1;
                    recoil1=3;
                }

                if(!turn && p2=="🧙"){
                    Console.Clear();
                    Console.WriteLine("Has usado Rayo Paralizador. Te saltaras el proximo turno del jugador 1");
                    Console.ReadKey();
                    i-=steps2-1;
                    recoil2=3;
                }

                if(turn && p1=="🧝"){
                    Console.Clear();
                    Console.WriteLine("Has usado Ocarina del Tiempo. Te teletransportaras a un arbol aleatorio");
                    Console.ReadKey();
                    Random rand = new Random();
                    int pos= rand.Next(trees.Count);
                    x1=trees[pos].Item1;
                    y1=trees[pos].Item2;
                    recoil1=2;
                }

                if(!turn && p2=="🧝"){
                    Console.Clear();
                    Console.WriteLine("Has usado Ocarina del Tiempo. Te teletransportaras a un arbol aleatorio");
                    Console.ReadKey();
                    Random rand = new Random();
                    int pos= rand.Next(trees.Count);
                    x2=trees[pos].Item1;
                    y2=trees[pos].Item2;
                    recoil2=2;
                }

                continue;
            }

            else{ i--; continue;}

            if (!ValidCell(new_x, new_y)){i--;continue;}
            if(grid[new_x, new_y] == "⬜" && (turn ? (!hab1 || p1!="🧌") : (!hab2 || p2!="🧌"))){i--; continue;}
            if (turn && mp && new_x == x2 && new_y == y2 && (!hab1 || p1!="🧟")){i--; continue;}
            if (!turn && mp && new_x == x1 && new_y == y1 && (!hab2 || p2!="🧟")){i--; continue;}

            if (turn)
            {
                if (grid[new_x, new_y] == "🌳")
                {
                    if(p1!="🧌" || !hab1){
                    e1 =Math.Max(0, e1-10);
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Has atravesado un bosque. -10 ⚡");
                    Console.ReadKey();
                    }
                }
                if (grid[new_x, new_y] == "🗻")
                {
                    if(p1!="🧌" || !hab1){
                    e1 =Math.Max(0, e1-20);
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Escalaste una colina. -20 ⚡");
                    Console.ReadKey();
                    }
                }
                if (grid[new_x, new_y] == "👹")
                {
                     if(p1!="🧌" || !hab1){
                    hp1 -= 20;
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Luchaste contra un demonio. -20 ❤️");
                    Console.ReadKey();
                     }
                }
                if (grid[new_x, new_y] == "💣")
                {
                     if(p1!="🧌" || !hab1){
                    hp1 -= 10;
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Detonaste una bomba. -10 ❤️");
                    Console.ReadKey();
                     }
                }

                if (hp1 <= 0)
                {
                    new_x = start_x1;
                    new_y = start_y1;
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("El jugador 1 ha muerto.");
                    Console.ReadKey();
                    hp1 = total_hp1;
                    e1 = total_e1;
                }

                else if (e1 <= 0)
                {
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("El jugador 1 se ha desmayado (3 turnos restantes)");
                    desmayo1 = 3;
                    Console.ReadKey();
                }

                x1 = new_x;
                y1 = new_y;

                if(x1==x2 && y1==y2 && p1=="🧟" && hab1){
                    x2=start_x2;
                    y2=start_y2;
                    hp2=total_hp2;
                    e2=total_e2;
                    hab1=false;
                }

                if(p1=="🧌" && hab1){
                  grid[x1, y1]="⬛";
                  hab1=false;
                }

                else if(grid[x1, y1]=="👹" || grid[x1, y1]=="💣")grid[x1, y1]="⬛";
            }

            else
            {
                if (grid[new_x, new_y] == "🌳")
                {
                     if(p2!="🧌" || !hab2){
                    e2=Math.Max(0, e2-10);
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Has atravesado un bosque. -10 ⚡");
                    Console.ReadKey();
                     }
                }
                if (grid[new_x, new_y] == "🗻")
                {
                    if(p2!="🧌" || !hab2){
                    e2 =Math.Max(0, e2-20);
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Escalaste una colina. -20 ⚡");
                    Console.ReadKey();
                    }
                }
                if (grid[new_x, new_y] == "👹")
                {
                    if(p2!="🧌" || !hab2){
                    hp2 -= 20;
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Luchaste contra un demonio. -20 ❤️");
                    Console.ReadKey();
                    }
                }
                if (grid[new_x, new_y] == "💣")
                {
                    if(p2!="🧌" || !hab2){
                    hp2 -= 10;
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("Detonaste una bomba. -10 ❤️");
                    Console.ReadKey();
                    }
                }

                if (hp2 <= 0)
                {
                    new_x = start_x2;
                    new_y = start_y2;
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("El jugador 2 ha muerto.");
                    Console.ReadKey();
                    hp2 = total_hp2;
                    e2 = total_e2;
                }

                else if (e2 <= 0)
                {
                    Console.Clear();
                    PrintMaze();
                    Console.WriteLine("El jugador 2 se ha desmayado (3 turnos restantes)");
                    desmayo2 = 3;
                    Console.ReadKey();
                }

                x2 = new_x;
                y2 = new_y;

                if(x1==x2 && y1==y2 && p2=="🧟" && hab2){
                    x1=start_x1;
                    y1=start_y1;
                    hp1=total_hp1;
                    e1=total_e1;
                    hab2=false;
                }

                if(p2=="🧌" && hab2){
                  grid[x2, y2]="⬛";
                  hab2=false;
                }

                else if(grid[x2, y2]=="👹" || grid[x2, y2]=="💣")grid[x2, y2]="⬛";
            }
            }

             if ((x1 == rows - 2 && y1 == col - 2) || (x2 == rows - 2 && y2 == col - 2)) break;

            if (mp) turn = !turn;

            Console.Clear();
            PrintMaze();
            Console.WriteLine("Fin del turno");
            Console.ReadKey();
        }

        Console.Clear();
        Console.WriteLine("Felicidades! Has completado el Laberinto");

        Console.ReadKey();
        Console.Clear();
        Environment.Exit(0);
    }

    public void PrintMaze()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (i == x1 && j == y1) Console.Write(p1);
                else if (i == x2 && j == y2) Console.Write(p2);
                else if (j == col - 1) Console.WriteLine(grid[i, j]);
                else Console.Write(grid[i, j]);
            }
        }
    }
}