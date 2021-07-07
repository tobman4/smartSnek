using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartSnek {
    interface IgameController {
        void Left();
        void Right();
        void Up();
        void Down();

        CellState[,] getMap();
        Point getHead();
        Point[] getSnake();
    }
}
