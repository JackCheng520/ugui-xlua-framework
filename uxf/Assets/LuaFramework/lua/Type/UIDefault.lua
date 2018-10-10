--UI 挂载节点
    BaseUIType =
    {
        None = 0, --挂载在root下
        Normal = 1, --Normal 节点
        Message = 2, --Message 节点
    }

--UI 背景or碰撞框
    UICollider = 
    {
        None = 0,       --无碰撞
        Normal = 1,     --只有碰撞盒子、
        WithGrayBg = 2, --统一灰色背景(九宫格)
        WithBg = 3,     --统一背景图片
    }

--UI 预设接在方式
    UILoadType = 
    {
        AssetBundle = 0,--assetbundle 方式
        Prefab = 1,     --Resources.Load 方式
    }

    GameUIType = 
    {
        GameUI_MainUI = 0,
        GameUI_MessageBoxUI = 1,
        GameUI_BagUI = 2,
    }