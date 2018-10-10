using UnityEngine;
using System.Collections;

namespace Game
{
    public class DebugUtil
    {
        public const int ERROR = 0;
        public const int WARNING = 1;
        public const int LOG = 2;

        public static bool enable = true;



        public static void DrawGrids()
        {
            if (!enable) return;

            //if (RoleController.instance != null && RoleController.instance.heroVo != null && RoleController.instance.heroVo.rolePrefab != null)
            //{
            //    int mapWidth = GameController.instance.cellData.mapWidth;
            //    int mapHeight = GameController.instance.cellData.mapHeight;

            //    int cellSizeX = GameController.instance.cellData.cellSizeX;
            //    int cellSizeY = GameController.instance.cellData.cellSizeY;

            //    int numX = (mapWidth * 2 + cellSizeX - 1) / cellSizeX;
            //    int numY = (mapHeight * 2 + cellSizeY - 1) / cellSizeY;

            //    if (numX % 3 != 0)
            //    {
            //        numX += 3 - numX % 3;
            //    }
            //    if (numY % 3 != 0)
            //    {
            //        numY += 3 - numY % 3;
            //    }

            //    float x = -numX * cellSizeX * 0.5f;
            //    float y = RoleController.instance.heroVo.rolePrefab.transform.position.y;
            //    float z = -numY * cellSizeY * 0.5f;

            //    Vector3 from = Vector3.zero;
            //    Vector3 to = Vector3.zero;
            //    for (int i = 0; i <= numX; i++)
            //    {
            //        from = new Vector3(x + i * cellSizeX, y, z);
            //        to = new Vector3(x + i * cellSizeX, y, z + numY * cellSizeY);
            //        Debug.DrawLine(from, to, Color.red, 0.1f);
            //    }


            //    for (int i = 0; i <= numY; i++)
            //    {
            //        from = new Vector3(x, y, z + i * cellSizeY);
            //        to = new Vector3(x + numX * cellSizeX, y, z + i * cellSizeY);

            //        Debug.DrawLine(from, to, Color.red, 0.1f);
            //    }


            //    //玩家当前格子
            //    Vector3 pos = RoleController.instance.heroVo.rolePrefab.transform.position;
            //    int roleCellX = (int)((pos.x + numX * cellSizeX * 0.5f) * 10000 / (cellSizeX * 10000));
            //    int roleCellY = (int)((pos.z + numY * cellSizeY * 0.5f) * 10000 / (cellSizeY * 10000));
            //    //Debug.Log("cell x:" + roleCellX + "cell y:" + roleCellY);
            //    from = new Vector3(x + roleCellX * cellSizeX, y, z + roleCellY * cellSizeY);
            //    to = new Vector3(x + (roleCellX + 1) * cellSizeX, y, z + roleCellY * cellSizeY);
            //    Debug.DrawLine(from, to, Color.green, 0.1f);

            //    from = new Vector3(x + roleCellX * cellSizeX, y, z + roleCellY * cellSizeY);
            //    to = new Vector3(x + roleCellX * cellSizeX, y, z + (roleCellY + 1) * cellSizeY);
            //    Debug.DrawLine(from, to, Color.green, 0.1f);

            //    from = new Vector3(x + (roleCellX + 1) * cellSizeX, y, z + roleCellY * cellSizeY);
            //    to = new Vector3(x + (roleCellX + 1) * cellSizeX, y, z + (roleCellY + 1) * cellSizeY);
            //    Debug.DrawLine(from, to, Color.green, 0.1f);

            //    from = new Vector3(x + roleCellX * cellSizeX, y, z + (roleCellY + 1) * cellSizeY);
            //    to = new Vector3(x + (roleCellX + 1) * cellSizeX, y, z + (roleCellY + 1) * cellSizeY);
            //    Debug.DrawLine(from, to, Color.green, 0.1f);

            //    //玩家当前9宫
            //    //Debug.Log("cell x:" + roleCellX + "cell y:" + roleCellY);
            //    from = new Vector3(x + (roleCellX - 1f) * cellSizeX, y, z + (roleCellY - 1f) * cellSizeY);
            //    to = new Vector3(x + (roleCellX + 2) * cellSizeX, y, z + (roleCellY - 1f) * cellSizeY);
            //    Debug.DrawLine(from, to, Color.blue, 0.1f);

            //    from = new Vector3(x + (roleCellX - 1f) * cellSizeX, y, z + (roleCellY - 1f) * cellSizeY);
            //    to = new Vector3(x + (roleCellX - 1f) * cellSizeX, y, z + (roleCellY + 2) * cellSizeY);
            //    Debug.DrawLine(from, to, Color.blue, 0.1f);

            //    from = new Vector3(x + (roleCellX + 2) * cellSizeX, y, z + (roleCellY - 1f) * cellSizeY);
            //    to = new Vector3(x + (roleCellX + 2) * cellSizeX, y, z + (roleCellY + 2) * cellSizeY);
            //    Debug.DrawLine(from, to, Color.blue, 0.1f);

            //    from = new Vector3(x + (roleCellX - 1f) * cellSizeX, y, z + (roleCellY + 2) * cellSizeY);
            //    to = new Vector3(x + (roleCellX + 2) * cellSizeX, y, z + (roleCellY + 2) * cellSizeY);
            //    Debug.DrawLine(from, to, Color.blue, 0.1f);
            //}
        }

        public static void DrawRoleSkillDir()
        {
            if (!enable)
                return;
            //if (RoleController.instance == null || RoleController.instance.heroVo == null || RoleController.instance.heroVo.rolePrefab == null)
            //    return;


            //GL.Color(Color.red);
            //GL.Begin(GL.LINES);

            //GL.Vertex(RoleController.instance.heroVo.rolePrefab.transform.position);
            //GL.Vertex(RoleController.instance.heroVo.rolePrefab.transform.position + RoleController.instance.heroVo.castSkillDir * 10f);

            //GL.End();
        }

        public static void Log(string _str, int _type = DebugUtil.LOG)
        {
            if (!enable)
                return;

            switch (_type)
            {
                case LOG:
                    Debug.Log("[" + System.DateTime.Now + "]   logOut:[ " + _str + " ]");
                    break;
                case ERROR:
                    Debug.LogError("[" + System.DateTime.Now + "]   logOut:[ " + _str + " ]");
                    break;
                case WARNING:
                    Debug.LogWarning("[" + System.DateTime.Now + "]   logOut:[ " + _str + " ]");
                    break;
            }

        }

    }
}