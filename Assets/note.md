# 零、索引
## 1. 目录
1. [**创建房间预制体**](#一创建房间预制体)
2. [**设置地图配置表**](#二设置地图配置表)
3. [**生成地图**](#三生成地图)
4. [**生成房间之间的连线**](#四生成房间之间的连线)
5. [**实现随机地图**](#五实现随机地图)
6. [**泛型事件框架**](#六泛型事件框架)
7. [**场景加载**](#七场景加载)
8. [**保存地图场景**](#八保存地图场景)
9. [**房间进出逻辑**](#九房间进出逻辑)

**----------地图↑       ↓卡牌对战界面---------**

10. [**卡牌数据类**](#十卡牌数据类)
11. [**对象池**](#十一对象池)
12. [**制作卡牌库实现抽卡**](#十二制作卡牌库实现抽卡)
13. [**卡牌布局**](#十三卡牌布局)
14. [**抽卡动画**](#十四抽卡动画)
15. [**实现鼠标事件**](#十五实现鼠标事件)
16. [**实现洗牌逻辑**](#十六实现洗牌逻辑)

**----------卡牌对战界面↑       ↓卡牌效果与人物---------**

17. [**导入Spine人物素材**](#十七导入spine人物素材)
18. [**人物基类代码**](#十八人物基类代码)
19. [**执行卡牌效果**](#十九执行卡牌效果)

**----------卡牌效果与人物↑       ↓UI和机制完善---------**

20. [**制作血条的UI Document**](#二十制作血条的ui-document)
21. [**回合转换**](#二十一回合转换)
22. [**出牌能量判断**](#二十二出牌能量判断)
23. [**防御牌及UI**](#二十三防御牌及ui)
24. [**力量牌 => buff增伤乘区**](#二十四力量牌--buff增伤乘区)

**----------UI和机制完善↑       ↓敌人、胜负与抽卡---------**

25. [**敌人ai意图**](#二十五敌人ai意图)
26. [**对战胜负逻辑**](#二十六对战胜负逻辑)
27. [**抽卡逻辑**](#二十七抽卡逻辑)

**----------敌人、胜负与抽卡↑       ↓收尾工作---------**

28. [**开始一局新的游戏**](#二十八开始一局新的游戏)
29. [**淡入淡出**](#二十九淡入淡出)
30. [**打包游戏**](#三十打包游戏)

## 2. 杂碎的知识点
1. [**枚举类型在unity编辑器中实现多选**](#1地图配置文件的数据结构mapconfigso)
2. [**屏幕宽高比**](#2地图生成器mapgenerator)
3. [**在Inspector中调用函数**](#2生成地图)
4. [**LineRenderer**](#1制作线的预制体lineprefab)
5. [**创建会动的虚线样式**](#1制作线的预制体lineprefab)
6. [**[TextArea]**](#1使用scriptobj传递数据)
7. [**使用ScriptObj传递数据**](#1使用scriptobj传递数据)
8. [**自定义Editor**](#4自定义editor)
9. [**序列化Vector3(通用脚本)**](#2序列化vector3通用脚本)
10. [**对象池**](#十一对象池)
11. [**IPointerEnterHandler,IPointerExitHandler**](#1制作划入划出事件效果)
12. [**IBeginDragHandler, IDragHandler, IEndDragHandler**](#2制作拖拽事件)
13. [**计算贝塞尔曲线**](#3攻击牌的拖拽指针)
14. [**导入Spine人物素材**](#十七导入spine人物素材)
15. [**UI Document**](#二十制作血条的ui-document)
16. [**新方法FindObjectsByType<T>()**](#1characterdeadevent-通知对局中谁死了)
17. [**在Awaitable中实现等待**](#1淡入淡出)
18. [**SceneLoadMgr中的新方法(**需要unity6支持，即2023.3之后**)**](#1sceneloadmgr中的新方法需要unity6支持即20233之后)
19. [**打包游戏**](#三十打包游戏)
20. [**常用汉字表**](#5常用汉字表2500字)

## 3.~~个人~~惯用缩写

用于防止看不懂,~~以及方便我摸鱼()~~

- background - bg
- Enemy - enm
- Effect - eff
- Value - val
- current - cur
- object - obj
- column - col
- position - pos
- rotation - rot
- damage - dmg
- library - lib
- image - img
- animation - ani
- manager - mgr
- button - btn

---

# 一、创建房间预制体
## 1.预备
- 做了个地图按钮
- 调整图层
  - Background
  - Character
  - front
- 碰撞体检测点击

## 2.房间脚本
- 房间的数据结构`RoomDataSO`
    - 图标
    - 类型
        - MinorEnm
        - EliteEnm
        - Shop
        - Treasure
        - Restroom
        - Boss
    - 房间场景
        *`使用Addressable插件存储场景变量`*

- 房间脚本
    - 变量
        - 房间状态的枚举类型
            - Locked
            - Visited
            - Attainable
        - 房间位置
            - row
            - col
        - 房间图标SpriteRenderer
    - 事件
        - Awake
            - 获取SpriteRenderer加载图标  
        - 点击时事件
        - SetupRoom
            - 外部创建房间时调用配置房间

[**回到目录索引**](#零索引)

-------

# 二、设置地图配置表
## 1.地图配置文件的数据结构MapConfigSO
1. ``List<RoomBlueprint>``
2. `RoomBlueprint` 类
    - 表示地图每一纵列出现的房间数量和类型
    - 变量：
        - min，max
        - Roomtype
    - *要让类出现在unity编辑器中在类上面写[Serializable]*
3. **枚举类型在unity编辑器中实现多选**
    1. 在枚举类上面写上`[Flags]`的Attribute => 此时实现多选，但是选出来的结果是乱的
    2. 设定枚举变量的值为**2^n**

## 2.地图生成器MapGenerator
1. 脚本MapGenerator
    - 变量
        - 地图配置文件MapConfig
        - 房间预制体Roomprefab
        - 屏幕宽高
        - 每一列的宽度
        - 地图生成起始点位置（Vector2）
        - *边界距离border（下面生成地图用）*
        - *存储房间的列表Rooms（下面生成地图用）*
    - 方法
        - 创建地图
            - 每一列读取配置文件的蓝图类得数据生成房间预制体
        - **屏幕的宽高**
            - `screenHeight = Camera.main.orthographicsize * 2`
            - `screenWidth = screenHeight * Camera.main.aspect(宽高比) `

[**回到目录索引**](#零索引)

---

#  三、生成地图
## 1.坐标计算方式(屏幕最中间为(0,0))
1. x: `-screenWidth/2 + border + screenWidth/(amount+1)`
2. y：`screenHeight/2 - screenHeight/(amount+1)*i ` *i为第i个房间*

## 2.生成地图
- **在Inspector中调用函数**
    - 声明：在函数头上面写`[ContextMenu("name")]`
    - 使用： 在编辑器中脚本组件上面右键

## 3.期望：最后的房间在靠近右侧的地方，使地图看的顺眼

[**回到目录索引**](#零索引)

---

# 四、生成房间之间的连线
## 1.制作线的预制体LinePrefab
- **LineRenderer**
    - 需要设置：
        - index：点坐标
            - 脚本中用setPosition设置坐标
        - width：线宽（3d空间展示）
        - material：线的样式
            - **创建会动的虚线样式**
                - shader：unlit/transparent
                - tiling：调整虚线线宽
                - 让虚线动起来：使材质的offset一直增加 => 改lineRenderer.material.mainTextureOffset(Vector2类)
    - 此处线的layer为Background

## 2.生成地图时连线：在mapGenerator
- 创建前一列房间列表与当前列房间列表

```c#
List<Room> previouscolRoom;
List<Room> currentcolRoom;
```

思路：**如果不是第一列的话，当前列与前一列开始连线,不一定全部是通路，但不能重复连线**
==> 需要一个函数来帮我们连线

```c#
void CreateConnection(List<Room> col1,List<Room> col2)
{
    HashSet<Room> connectedCol2Room = new(); //已经连上线的第二列的房间
    //下面实现随机路径连线
}
```

- 随机路径连线方法

```c#
/// <summary>
/// 此函数能随机连一个房间，并返回该房间
/// </summary>
/// <param name="room">起点房间</param>
/// <param name="col2">终点房间所在列</param>
/// <returns>返回该房间，将他加到hashset中</returns>
Room ConnectToRandomRoom(Room room,List<Room> col2);
```

- 目前出现的问题：
    - 有些房间没有获得连线 => l1-l2连了一次，l2-l1在连一次
    - 重新生成地图时没有清除线

[**回到目录索引**](#零索引)

---

# 五、实现随机地图
## 1.创建场景，把他标记为addressable

## 2.修改场景的group

**这样就有场景了**

## 3.在生成room之后找到room对应的类型然后调用SetupRoom
- 在mapGenerator中添加变量
    - `List<RoomDataSO>`
    - `Dictionary<RoomType,RoomDataSO>`
- 通过mapConfig获取数据setup房间
    - `GetRandomRoomType()`


[**回到目录索引**](#零索引)

---

# 六、泛型事件框架
## 1.使用ScriptObj传递数据
- 整个过程：
    1. 监听端注册要调用的事件
    2. 创建广播端
    3. 广播端调用事件被监听端发现
    4. 监听端调用被注册函数

- 基类ScriptObj：`BaseEventSO`
    - UnityAction
        - e.g.: Button的OnClick事件
        - 用于监听事件
    - **[TextArea]**
        - 用于添加事件描述
        - 添加在变量前面
    - 变量
        - `[TextArea]string description` 事件描述
        - `UnityAction<T> OnEventRaised` 存放事件
        - `string lastSender` 上一个呼叫的事件
    - 方法
        ```c#
        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="sender">呼叫的事件</param>
        public void RaiseEvent(T value,object sender)
        {
            OnEventRaised?.Invoke(value);
            lastSender = sender.ToString();
        }
        ```

        触发事件

- 基类的监听`BaseEventListener`
    - **用于监听对应的SO事件**
    - 相比于之前每次都要在代码中注册事件，这次直接在基类中注册，减少了冗余代码
    - 变量
        - `BaseEventSO<T> eventSO` 监听的事件
        - `UnityEvent response` 反馈启动的事件
    - 方法
    ```c#
    private void OnEnable() {
        eventSO.OnEventRaised += OnEventRaised;
    }

    private void OnDisable() {
        eventSO.OnEventRaised -= OnEventRaised;
    }
    ```

## 2.更通用的事件类型
- `ObjectEventSO`
    - 继承自`BaseEventSO`,可以使传递的数据类型更通用
- `ObjectEventListener`
    - 继承自`BaseEventListener`，`ObjectEventSO`的监听

## 3. `LoadRoomEvent`
- 点击广播房间加载对应场景，传递RoomData数据

## 4.**自定义Editor**
- 创建脚本后输入editor出框架
    - 框架长这样：
    ```c#
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(ObjectEventSO))]
    public class ObjectEventSOEditor : BaseEventSOEditor<object> {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
        }
    }
    ```

- `EditorGUILayout.LabelField(listener.ToString());`用于在编辑器中显示什么东西

```c#
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(BaseEventSO<>))] //指定修改的类型
public class BaseEventSOEditor<T> : Editor {
    BaseEventSO<T> baseEventSO; //谁订阅了事件

    private void OnEnable() {
        if (baseEventSO == null)
        {
            baseEventSO = target as BaseEventSO<T>;
            //target是Editor包中的属性，官方描述是"object being inspected",将它转化成想用的类型
        }
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        EditorGUILayout.LabelField("订阅数量："+ GetListener().Count);
        foreach (var listener in GetListener())
        {
            EditorGUILayout.LabelField(listener.ToString()); //获取监听名字
        }
    }

    List<MonoBehaviour> GetListener()//获取监听器
    {
        List<MonoBehaviour> listeners = new();

        if (baseEventSO == null || baseEventSO.OnEventRaised == null)//没有监听返回空列表
            return listeners;

        var subscribers = baseEventSO.OnEventRaised.GetInvocationList();//获得所有委托，也就是所有事件
        foreach (var subscriber in subscribers) {
            var obj = subscriber.Target as MonoBehaviour;//是monobehavior就加到监听列表
            if (!listeners.Contains(obj))
            {
                listeners.Add(obj);
            }
        }
        return listeners;
    }
}

```

[**回到目录索引**](#零索引)

---

# 七、场景加载
## 1.SceneLoadMgr中的新方法(**需要unity6支持，即2023.3之后**)

*备注：我在写这个项目的时候使用了协程加载场景，写法大差不差*

- 变量
    - `AssetReference currentScene`
    - `public AssetReference map`
- 方法
    ```c#
    /// <summary>
    /// 异步加载场景，调用方法使用await LoadSceneTask();
    /// </summary>
    /// <returns></returns>
    async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Addictive);//以LoadSceneMode.Addictive方式异步加载
        await s.Task;//等待任务状态
        if(s.Status == AsyncOperationStatus.Succeed)//如果任务完成
        {
            //场景加载
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    /// <summary>
    /// 异步卸载场景，调用方法使用await UnloadSceneTask();
    /// </summary>
    /// <returns></returns>
    async Awaitable UnloadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());//异步卸载当前激活的场景
    }

    /// <summary>
    /// 异步加载地图，监听返回地图的事件函数
    /// </summary>
    /// <returns></returns>
    public async void LoadMap()
    {
        await UnloadSceneTask();

        currentScene = map;
        await LoadSceneTask();
    }
    ```

*写完后遇到的问题：返回地图的时候地图变了*


[**回到目录索引**](#零索引)

---

# 八、保存地图场景
## 1.MapLayoutSO存储地图布局
- 变量
    - `public List<RoomMapData> RoomMapDataList`
    - `public List<LinePos> LinePosList`
- 用于储存房间数据的数据结构RoomMapData
    - 变量(需要序列化)
        - `public float posx,posy` 位置
        - `public int row,col` 行列
        - `public RoomDataSO data` 数据
        - `public RoomState state` 状态
- 用于储存线起点和终点的数据结构LinePos
    - 变量(需要序列化)
        - `SerializeVector3 startPos,endPos` 起点和终点(下文提及)

## 2.序列化Vector3(通用脚本)
```c#
using UnityEngine;

[System.Serializable]
public class SerializeVector3
{
    public float x, y, z;
    public SerializeVector3(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

```

## 3.开始储存数据

- 存储地图
    1. 初始化列表
    2. 遍历地图所有房间，生成`RoomMapData`,并添加到列表中
    3. 遍历地图所有连线，生成`LinePos`,并添加到列表中
- 加载地图
    1. 读取房间数据
    2. 生成房间并setup
    3. 读取线
    4. 生成线

[**回到目录索引**](#零索引)

---

# 九、房间进出逻辑

**目标：实现开始时可以点第一排进入，通过房间后该排所有房间禁止进入，与当前房间连线的房间可以进入**

## 1.创建地图时重置状态，实现开始时可以点第一排进入

## 2.在Room类中创建`List<Vector2Int> LinkTo`列表，存放关系

## 3.创建GameMgr管理游戏进度，在通过房间后通知更新状态
- 变量 
    - `MapLayoutSO mapLayout`
- 方法
    ```c#
        void UpdateMapLayoutData(object value)
        {
            Vector2Int roomVector = (Vector2Int)value;
        }
    ```
- 在SceneLoadMgr中记录加载的房间，也只有这里能知道进了哪个房间
    - 需要更改OnLoadRoomEvent(object data)，因为没有包含`List<Vector2Int> LinkTo`
    - 添加AfterRoomLoadEvent，用于在进入房间后传递Vector2Int通知GameMgr在通过房间后更新状态
- 在MapLayoutSO中需要添加`List<Vector2Int> LinkTo`

## 4.做点视觉特效区分房间状态
- 在SetupRoom中
    - 根据房间状态改变spriteRenderer的颜色

**自此，地图部分完工**

[**回到目录索引**](#零索引)

---

# 十、卡牌数据类
## 1. 制作卡牌Prefab

## 2.卡的数据CardDataSO
- 变量
    - `string cardName`
    - `Sprite cardImg`
    - `int cost`
    - `string description`[TextArea]
    - `CardType type`枚举变量
        - Attack
        - Defense
        - Abilities

## 3.卡的行为控制脚本Card
- 变量
    - `CardDataSO cardData`
    - Header:组件
        - `SpriteRenderer spriteRenderer`
        - `TextMeshPro cost,type,description`
- 方法
    - `void Init(CardDataSO data)`初始化函数

[**回到目录索引**](#零索引)

---

# 十一、**对象池**

**固定写法，可通用于其他项目**

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject prefab;//要进对象池的prefab
    ObjectPool<GameObject> pool;//对象池

    private void Start() {
        //初始化对象池
        // public ObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, 
        // Action<T> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        // 构造器函数
        pool = new ObjectPool<GameObject>(
            createFunc:()=>Instantiate(prefab,transform),//创建对象池新物体执行的函数方法
            actionOnGet:(obj)=>obj.SetActive(true),//拿新物品执行的函数方法
            actionOnRelease:(obj)=>obj.SetActive(false),//放回物体到对象池的函数方法
            actionOnDestroy:(obj)=>Destroy(obj),//摧毁物体的函数方法
            collectionCheck:false,//是否检查对象
            defaultCapacity:10,//初始对象池容量，默认为10
            maxSize:100//最大容量，默认为10000
        );
        PreFillPool(7);
    }

/// <summary>
/// 初始化预先生成物体到对象池
/// </summary>
/// <param name="count">初始数量</param>
    void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();
        }

        foreach (var item in preFillArray)
        {
            pool.Release(item);
        }
    }
/// <summary>
/// 从对象池获取对象
/// </summary>
/// <returns>GameObject对象</returns>
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

/// <summary>
/// 把对象放回对象池
/// </summary>
/// <param name="obj">GameObject对象</param>
    public void ReturnObjectFromPool(GameObject obj)
    {
        pool.Release(obj);
    }
}

```

用法：用一个管理类来管理卡牌对象池 => `CardMgr`

- 变量
    - `PoolTool poolTool` 对象池
    - `List<CardDataSO> cardDataList` 游戏中所有可能出现的卡牌
- 方法
    - `InitCardDataList` 只要加到AddressableGroup中就能自动获取游戏中所有可能出现的卡牌
        - 使用Addressables.LoadAssetAsync并创建回调函数(`.Completed+=OnCardDataLoaded` 注册)

[**回到目录索引**](#零索引)

---

# 十二、制作卡牌库实现抽卡

目前的CardData只告诉了我们有什么种类的卡牌，并没有告诉我们有多少张
需要把CardData与张数合成一个新的数据库，即卡牌库

卡牌库分为两类
- 初始玩家卡牌库
- 当前游戏进度卡牌库

# 1.CardLibSO:卡牌库数据结构
- 变量
    - `List<CardLibEntry> cardLibList`
- `struct CardLibEntry` [System.Serializable]
        - `CardDataSO cardData`
        - `int amount`

# 2.在CardMgr中控制卡牌库
- 在Awake方法中：
    把`初始卡牌库`中的卡牌添加到`当前游戏进度卡牌库`
- 增加抽牌的对外函数
    - `GameObject GetCard()` 获取卡
    - `void DiscardCard(GameObject card)` 回收卡

# 3.CardDeck:管理对卡片的操纵
- 变量
    - `CardMgr cardMgr`
    - `List<CardDataSO> drawDeck` 抽牌堆
    - `List<CardDataSO> discardDeck` 弃牌堆
    - `List<Card> handCardObjList` 手牌(每回合)
- 方法
    - 初始化抽牌堆`InitDeck`
        - 牌从当前游戏进度卡牌库中来 
        - 洗牌/更新抽牌堆/弃牌堆的数字(下面会做)
    - 抽牌功能`DrawCard(int amount)`

[**回到目录索引**](#零索引)

---

# 十三、卡牌布局
## 1.CardLayoutMgr:管理卡牌布局
- 变量
    - `public bool isHorizental` 切换横向/扇形布局
    - `public float maxWidth = 7f` 横向最大宽度
    - `List<Vector3> cardPos` 卡牌位置
    - `List<Quaternion> cardRot` 卡牌朝向角度
    - `float cardSpacing = 2` 卡牌宽度
    - `public Vector3 centerPoint` 卡牌中心
    - 弧形参数
        - `public float angBetweenCards = 7.5f` 两卡牌间隔角度
        - `public float radius = 17f` 弧半径
- 方法
    - `CalcPos(int num,bool horizental)` 计算卡牌坐标位置
        - 横向布局
            - 计算所有间隙宽度：`totalWidth = min(maxWidth,cardSpacing * (num-1) )`
            - 计算现在每两张牌间的间隔(若只有一张则为0)
            - 计算x坐标：`xPos = -(totalWidth/2) + curSpacing*i`,i为卡牌数
        - 扇形布局
            - 计算扇面最右边的角度 `cardAng = (num-1) * angBetweenCards / 2`
            - 计算x坐标：`x = centerPoint.x - sin(cardAngle - i * angBetweenCards) * radius` i为第i张卡
            - 计算y坐标：`y = centerPoint.y - cos(cardAngle - i * angBetweenCards) * radius` i为第i张卡
            - 计算角度： `rot = Quaternion.Euler(0, 0, cardAngle - i * angBetweenCards)` i为第i张卡
    - `public CardTransform GetCardTransForm(int index,int totalCards)` 返回卡牌的坐标与朝向
        - struct CardTransform
            - Vector3 pos
            - Quaterion rot
        - return new CardTransform()
    
## 2.在CardDeck中
- 增加函数`void SetCardLayout()` 设置卡牌布局
        - 循环手牌排序

## 3.目前存在的问题
- 可以一直抽卡：
    查看CurrentCardLib时发现lib没有在游戏停止时初始化
    解决：在CardMgr的OnDisable时清空CurrentCardLib
- 卡牌叠层问题：
    在设置卡牌布局`SetCardLayout()`中改变叠层顺序
    `curCard.GetComponent<SortingGroup>().sortingOrder = i;`
- 当卡片 > 9 张时扇形布局显示会超出版面(这是我个人修改的，视频内没提到)
    在计算`cardAngle`前我先改变两卡片间隔角度`angBetweenCards = Mathf.Min(7.5f,50/num);`

[**回到目录索引**](#零索引)

---

# 十四、抽卡动画
## 1.在生成时把卡的scale设成0

## 2.在`SetCardLayout()`中制作动画
- `DoScale`把整体大小设为1

**这样就有了原地放大缩小的动画了**

## 3.移动动画
- CardDeck起始点 `Vector3 deckPos`
- `DoMOVE`从起始点移到终点

[**回到目录索引**](#零索引)

---

# 十五、实现鼠标事件
## 1.制作划入划出事件效果
- **IPointerEnterHandler,IPointerExitHandler**
    - 处理鼠标划入划出的*接口*
    - 不考虑用DoMove，因为动作太快会出bug TAT
- 存储原始数据
    - `Vector3 originalPos`
    - `Quaternion originalRot`
    - `int originalLayerOrder`
- 用于保存原始数据的方法`UpdateOriginData(Vector3 pos,Quaternion rot)`
    - `originalLayerOrder = GetComponent<SortingGroup>().sortingOrder`
    - 生成卡牌时调用

## 2.制作拖拽事件
- 使用专门的脚本CardDragHandler
    - 拖拽事件接口`IBeginDragHandler, IDragHandler, IEndDragHandler`
        - 分别是开始拖拽，正在拖拽，结束拖拽
    - 变量
        - `bool canMove`
        - `Card curCard`
        - `bool canExecute`
    - 方法
```c#

//判断牌是否可移动
    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (curCard.cardData.type)
        {
            case CardType.Attack:
                break;
            case CardType.Defense:
            case CardType.Abilities:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
            curCard.isAnimating = true;//拖拽时不执行划入划出事件
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);//z=10是因为摄像机的z坐标是-10
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);//屏幕坐标转化为世界坐标
            curCard.transform.position = worldPos;

            canExecute = worldPos.y > 1f;//可不可以执行卡牌效果
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canExecute)
        {
            //执行卡牌的效果
        }
        else
        {
            curCard.ResetCardPos();
            curCard.isAnimating = false;
        }
    }
```

## 3.攻击牌的拖拽指针
- 箭头线的prefab
- 控制箭头线脚本DragArrow
    - 变量
        - `LineRenderer lineRenderer`
        - `Vector3 mousePos`
        - `public int pointsCount` 贝塞尔曲线控制点的数量
        - `public float arcModifier` 描绘贝塞尔曲线的形状
    - 方法
        - **计算贝塞尔曲线**
        ```c#
        public void SetArrowPosition()
        {
            Vector3 cardPosition = transform.position; // 卡牌位置
            Vector3 direction = mousePos - cardPosition; // 从卡牌指向鼠标的方向
            Vector3 normalizedDirection = direction.normalized; // 归一化方向

            // 计算垂直于卡牌到鼠标方向的向量
            Vector3 perpendicular = new(-normalizedDirection.y, normalizedDirection.x, normalizedDirection.z);

            // 设置控制点的偏移量
            Vector3 offset = perpendicular * arcModifier; // 你可以调整这个值来改变曲线的形状

            Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset; // 控制点


            lineRenderer.positionCount = pointsCount; // 设置 LineRenderer 的点的数量

            for (int i = 0; i < pointsCount; i++)
            {
                float t = i / (float)(pointsCount - 1);
                Vector3 point = CalculateQuadraticBezierPoint(t, cardPosition, controlPoint, mousePos);
                lineRenderer.SetPosition(i, point);
            }
        }

        //计算二次贝塞尔曲线点
        Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0; // 第一项
            p += 2 * u * t * p1; // 第二项
            p += tt * p2; // 第三项

            return p;
        }
        ```


        > c#通用实现画贝塞尔曲线的方法：二次贝塞尔曲线
        > ```c#
        > float u=1-t;
        > float tt =t*t;
        > float uu = u *u;
        > ```
        > `u`是`t`的补码，即`1-t`。
        > `tt`是`t`的平方。
        > `uu`是`u`的平方
        > 
        > ```c#
        > Vector3 p=uu*p;// 第一项
        > p +=2*u*t*p1;// 第二项
        > p += tt * p2;// 第三项
        > return p;
        > ```
        > 贝塞尔曲线上的点`p`是三个控制点`p0`、`p1`和 `p2` 的加权平均值。
        > 权重由 `t` 和 `u` 决定。
        > 第一项代表 `p0` 对 `p`的贡献，权重为`uu`。
        > 第二项代表 `p1` 对 `p`的贡献，权重为`2*u*t`。
        > 第三项代表 `p2` 对 `p`的贡献，权重为`tt`.

- 在`CardDragHandler`中
    - 增加变量
        - `public GameObject arrowPrefab`
        - `GameObject curArrow`
    - 在`OnBeginDrag(PointerEventData eventData)`中添加生成`arrowPrefab`
    - 在`OnEndDrag(PointerEventData eventData)`中添加销毁`arrowPrefab`
- 目前存在的问题(视频中没解决)：在拖攻击牌到其他类型牌时其他牌会动
    - 或许未来通过控制CardMgr来解决

[**回到目录索引**](#零索引)

---

# 十六、实现洗牌逻辑 & 弃牌逻辑的事件函数
## 1.两种洗牌情况
1. 刚开始的时候洗牌
2. 抽牌堆中没有牌了，把弃牌堆的牌全部加入抽牌堆并洗牌

## 2.在CardDeck中增加
- 方法`ShuffleDeck`洗牌逻辑
    1. 先清理弃牌堆
    2. 对每张牌进行循环，让他与某个随机数的牌交换

## 3.弃牌逻辑的事件函数
- `DiscardCard(Card card)`
    1. 弃牌堆中加牌
    2. 手牌堆中弃牌
    3. 回收这张牌
    4. 重置手牌显示排序

**自此，卡牌对战界面逻辑完成(除了弃牌())**

[**回到目录索引**](#零索引)

---

# 十七、导入Spine人物素材
## 1.下载[spine-unity](https://zh.esotericsoftware.com/spine-unity-download)并导入

## 2.添加spine对象：找到spine自动生成的asset文件然后拖入选择`SkeletonMecanim`
- 理由是这样会创建一个animator，我对animator更熟一些

## 3.调Animator，以及animation

## 4. 完成

[**回到目录索引**](#零索引)

---

# 十八、人物基类代码
## 1.人物基类CharacterBase
- 变量
    - `public int maxHp`
    - `protected Animator animator`
    - `IntVariable hp`
    - `int curHP { get => hp.curValue,set => hp.SetValue(value); }`
    - `int maxHP { get => hp.maxValue; }`
    - `bool isDead`
- 方法    
```c#
/// <summary>
/// 获取组件
/// </summary>
protected virtual void Awake()
{
    animator = GetComponentInChildren<Animator>();
}
        
/// <summary>
/// 游戏一开始的时候赋值
/// </summary>
protected virtual void Start()
{
    hp.maxValue = maxHP;
    curHP = maxHP
}

/// <summary>
/// 人物受伤时扣血
/// </summary>
public void TakeDamage(int dmg)
{
    if(curHP > dmg)
        curHP -= dmg;
    else
    {
        curHP = 0;
        isDead = true;
    }      
}
```

## 2.IntVariable SO文件
- 用法
    - 当这个SO文件的值被改变时启动对应事件
    - **这个SO文件适用于读取变量而不是设置实际数值**
- 变量
    - `int maxValue`
    - `int curValue`
    - `IntEventSO ValChangeEvent`
    - `[TextArea] [SerializeField]string description`
- 方法
```c#
/// <summary>
/// 通用的方法，用于更新数值
/// </summary>
public void SetValue(int val)
{
    curValue = val;
    ValChangeEvent.RaiseEvent(val,this);
}
```

*`ObjectEventSO`虽然泛用，但是他的开销比较大，这个SO针对int值的传递更方便，开销更小*

- `IntEventSO`
    - 用于监听事件，广播端
    - 继承自`BaseEventSO<int>`
- `IntEventListener`
    - 用于监听事件，监听端
    - 继承自`BaseEventListener<int>`
- `IntEventSOEditor`
    - 用于编辑器显示
    - 继承自`BaseEventSOEditor<int>`

[**回到目录索引**](#零索引)

---

# 十九、执行卡牌效果

*流程：打出牌 => 找到敌人 => 触发效果 => 回收牌*

## 1.Effect*抽象*基类 SO文件
- 变量
    - `int value`
    - `EffectTargetType tarType`
        - `self`
        - `target`
        - `ALL`
- 方法
    - `public abstract void Execute(CharacterBase self,CharacterBase tar)` 声明

## 2.攻击效果`DamageEff`
- **继承自`Effect`**
- 方法
    - `public override void Execute(CharacterBase self,CharacterBase tar)` 

## 3.CardDataSO
- 实现卡牌的实际效果
    - 增加变量
        - `List<Effect> cardEffs` 卡牌效果的列表

## 4.CardDragHandler
- 攻击牌的箭头找到敌人 => Execute
    - 增加变量 `CharacterBase tarCharacter`
    - 在`onDrag`方法
        - `canMove`之外就是攻击牌发动的时候
        - 通过方法的传参判断鼠标进入了哪个GameObject
        - 改变`canExecute = true`
        - 实际执行方法放到Card中

## 5.Card
- 增加变量
    - `Player player` 记录玩家
        - `Player`继承自`CharacterBase`
        - 初始化时赋值
- 增加方法
    - `public void ExecuteCardEff(CharacterBase self,CharacterBase tar)` 卡牌效果实际执行方法
        - 遍历卡牌数据中的效果列表，调用`cardData.Execute()`

## 6.回收牌
- 增加事件`ObjectEventSO DiscardCardEvent`
    - 广播端：`Card`
    - 监听端：`CardDeck`

**自此，卡牌效果与人物完工**
**弃牌逻辑也全部完成了**

[**回到目录索引**](#零索引)

---

## 二十、制作血条的UI Document
# 1.UI Document的工作流程
顺序由上到下
|  流程  | unity |  web前端 |
|  ----  | ----  |  ----  |
| 内容布局| uxml |  html  |
| 样式设计|  uss |  css    |
|  UI行为 |  c#  |JavaScript|

# 2.创建UI Document
`UI Toolkit` - `UI Document`

# 3.Panel Settings
- `Scale Mode`
    - 调整ui尺寸
    - 此处调整为适配屏幕

# 4.使用
在`GameObject`挂载`UI Document`组件,之后添加`Source Asset`与`Panel Settings`

# 5.编写UI
- 界面
    - Flex
        - Grow
            - 值为1时自动填充整个UI界面
    - Position
        - 原点在UI的左上角 
        - Position Mode
            - Relative 相对位置
            - Absolute 绝对位置 => 可以拖拽改变位置
    - Library
        - Progress Bar模板 做血条
    - Display
        - Display Style
            - Flex 显示
            - None 隐藏
    - Attribute
        - Value 值
        - Title 显示的文字
    - Transition Animation 变化动画
        - Property 监测的变量
        - Duration 动画持续时间
        - Easing 动画效果
            - [此网站能查看动画效果](https://easings.net/zh-cn)
- 行为脚本
    - HealthBarController
        - 变量
            - `public Transform HpBarTransform`
            - `UIDocument HpBarDocument`
            - `ProgressBar HpBar`
        - 方法
            - 找到HpBar
                - 在UI Document中为Progress Bar起名字
                - 调用UI Document的`Q<ProgressBar>`方法
                - 改hpbar的最大值
            - UI移到玩家头顶上
                - UI坐标使用`Rect`
                - 用Panel Settings的`RuntimePanelUtils.CameraTransformWorldToPanelRect()`获取Rect坐标
                - 赋值

# 6.绑定数据
- HealthBarController中添加
    - 变量
        - `CharacterBase curCharacter`
    - 方法
        - 更新血条`UpdateHpBar`
            - 看UI builder的变量是什么，脚本里就能相应调用

# 7.血条样式StyleSheet
- 编写uss文件
    - [查看unity官方教程](https://docs.unity3d.com/cn/2022.3/Manual/UIE-about-uss.html)
    - 这个东西是类似于css的新语言，要自己了解
    - 写uss文件时可以在编辑器挂css写

*Gameplay Panel的写法与血条差不多，故省略*

[**回到目录索引**](#零索引)

---

# 二十一、回合转换

**这里已经完成了GamePlayPanel**

## 1.TurnBaseMgr
- 用途
    - 管理所有的回合转换
- 变量
    - `bool isPlayerTurn = false`
    - `bool isEnmTurn = false`
    - `public bool battleEnd = true`
    - `float timeCounter`
    - `public float enmTurnDuration` 等待时间
    - `public float playerTurnDuration` 等待时间
- 方法
    - `Update`
        - 回合结束就`return`
        - 如果是敌人的回合
            - 敌人计时
            - 计时结束，玩家回合开始
        - 玩家回合同理
    - `EnmTurnEnd` 敌人回合结束时发生的事件
    - `EnmTurnBegin` 敌人回合开始时发生的事件
    - `PlayerTurnBegin` 玩家回合开始时发生的事件
        - 通知`CardDeck`抽牌
```c#
[ContextMenu("GameStart")]
public void GameStart()
{
    isPlayerTurn = true;
    isEnmTurn = false;
    battleEnd = false;
    timeCounter = 0f;
}
```
- 监听
    - 监听`GamePlayPanel`，当结束回合按钮被按下时弃掉玩家手中所有的牌

# 2.在`CardDeck`中
- 增加监听
    - 玩家回合结束后弃掉玩家手上所有的牌

# 3.目前的问题
- 结束回合的按钮可以一直被按，导致一直处于敌方回合，上面的回合显示文字要改
    - `GamePlayPanel`中
        - 增加`OnEnmRoundBegin`方法
            - 设置按钮不可用
            - 改变文字及颜色

[**回到目录索引**](#零索引)

---

# 二十二、出牌能量判断
## 1.Player脚本
- 变量
    - `IntVariable playerMana`
    - `public int maxMana`
    - `public int curMana { get => playerMana.curValue; set => playerMana.SetValue(value); }`
- 方法
    - OnEnable
        - 初始化数据
    - NewTurn 每一回合开始执行的事件，监听事件函数
        - Mana回满
    - UpdateMana 
        - 使用牌后更新魔力值

## 2.GamePlayPanel
- 增加监听
    - 用于打出牌后更新魔力值

## 3.卡牌消耗逻辑
- 在Card中
    - 用牌时IntEventSO通知Player消耗魔力

## 4.出牌能量判断
- 显示与状态
    - 在Card中
        - 增加变量
            - `Player player`
            - `bool isAvailable`
        - 增加方法
            - `public void UpdateCardState`
                - 在`CardDeck`中的`SetCardLayout`中更新
- 实际出牌
    - 在所有的拖拽过程检测`isAvailable`,若为`false`就return

[**回到目录索引**](#零索引)

---

# 二十三、防御牌及UI
## 1.IntVariable Defense 存储防御值
- 在CharacterBase中添加
    - 变量
        - IntVariable Defense
    - 方法
        - 更新
            - DefenceExecute
        - 重置
            - 游戏启动时 
            - 玩家回合开始时

## 2.DefenceEff
- 继承自Effect

## 3.UI
- 在HealthBarController中改
    - UpdateHealthBar
        - 更新防御数值：防御大于0显示

## 4.防御承伤
- 在CharacterBase中
    - 改TakeDamage方法
        - 伤害-防御
        - 防御值更新

*回血逻辑与防御差不多*

[**回到目录索引**](#零索引)

---

# 二十四、力量牌 => buff增伤乘区

*此项目的增伤不超过150%，多次使用增伤牌会叠加buff回合数*

## 1.在CharacterBase中添加
- 增伤乘区变量以及持续回合数
    - `IntVariable buffRound`
    - `float baseStrength = 1f;`
    - `float strengthEff = 0.5f;`
- 方法
```c#
    /// <summary>
    /// 给自己加增伤
    /// </summary>
    /// <param name="round">持续回合</param>
    /// <param name="isPositive">是buff还是debuff</param>
    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = strengthEff + baseStrength;

            baseStrength = Mathf.Min(1.5f, newStrength);
            //启动动画
            buff.SetActive(true);
        }
        else
        {
            float newStrength = baseStrength - strengthEff;

            baseStrength = Mathf.Min(1.5f, newStrength);
            //启动动画
            debuff.SetActive(true);
        }

        var curRound = buffRound.curValue + round;

        if (baseStrength == 1)
        {
            buffRound.SetValue(0);
        }
        else
        {
            buffRound.SetValue(curRound);  
        }
    }

/// <summary>
/// 回合转换事件函数
/// </summary>
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(Math.Max(buffRound.curValue - 1,0));
        if (buffRound.curValue <= 0)
            baseStrength = 1;
    }
```

## 2.StrengthEffect
```c#
using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "Effect/StrengthEffect")]
public class StrengthEffect : Effect
{
    public override void Execute(CharacterBase self, CharacterBase tar)
    {
        switch (tarType)
        {
            case EffectTargetType.self:
                self.SetupStrength(value,true);
                break;
            case EffectTargetType.target:
                tar.SetupStrength(value,false);
                break;
            default:
                break;
        }
    }
}
```

## 3.挂事件，更新ui，和前面的一样

**自此，UI和机制完善部分完成**

[**回到目录索引**](#零索引)

---

# 二十五、敌人ai意图
## 1.EnmActionDataSO
- 用于储存敌方的攻击意图的配置文件
- 变量
    - `List<EnmAction> Actions`
    - `struct EnmAction`[System.Serializable]
        - `Effect effect`
        - `Sprite intent`

## 2.Enemy脚本
- 变量
    - `EnmActionDataSO actionDataSO`
    - `public EnmAction curAction`
    - `protected Player player`
- 方法
    - 重写Awake，获取player
    - `public virtual void OnPlayerTurnBegin()`
        - 使用virtual是为了将来计划定制化敌人可以重载函数
        - 玩家回合开始时在现在的行动意图列表随机获取一个行动

## 3.在HpBarController中
- 增加方法
    - SetIntentElement
        - 玩家回合开始时修改UI
    - HideIntentElement
        - 敌人回合结束时隐藏UI

## 4.执行敌人意图
- 敌人回合开始时执行`curAction`的effect
    - 在Enemy脚本中
        ```c#
        public virtual void OnEnmTurnBegin()
        {
            switch (curAction.effect.tarType)
            {
                case EffectTargetType.self:
                    Skill();
                    break;
                case EffectTargetType.target:
                    Attack();
                    break;
                default:
                    break;
            }
        }

        public virtual void Skill()
        {
            curAction.effect.Execute(this,this);
        }

        public virtual void Attack()
        {
            curAction.effect.Execute(this,player);
        }
        ```

[**回到目录索引**](#零索引)

---

# 二十六、对战胜负逻辑

*由GameMgr管理*

## 1.CharacterDeadEvent 通知对局中谁死了
- 监听
    - GameMgr
        - 增加变量
            - `List<Enm> aliveEnmList`
            - `ObjectEventSO gameWinEvent`
            - `ObjectEventSO gameLoseEvent`
        - `OnCharacterDeadEvent`
            - 判断传入的变量类型来判断游戏的胜负
                - 玩家
                    - 广播 `gameLoseEvent`
                        - 使用协程延迟执行
                - 敌人
                    - 把他从`aliveEnmList`中移除
                    - 如果`aliveEnmList`为空，则通知对局胜利`gameWinEvent`
                        - 使用协程延迟执行
        - `OnRoomLoadedEvent`
        ```c#
        /// <summary>
        /// 在房间加载后获取敌人，添加到`aliveEnmList`中
        /// </summary>
        /// <param name="obj"></param>
            public void OnLoadRoomEvent(object obj)
            {
                //新方法FindObjectsByType<Enm>
                //第一个变量是需不需要找没有激活的obj，可选FindObjectsInactive.Include包含 | FindObjectsInactive.Exclude不包含
                //第二个变量是返回列表的排序模式，可选FindObjectsSortMode.None不排序 | FindObjectsSortMode.InstanceID 按InstanceID排序
                var enms = FindObjectsByType<Enm>(FindObjectsInactive.Include,FindObjectsSortMode.None);

                foreach (var item in enms)
                {
                    aliveEnmList.Add(item);
                }
            }
        ```

        *在unity 2022.3编辑器中这个方法会导致enms返回空数组，经过多次试验为代码加载顺序问题，因此作出以下更改*

        ```c#
        /// <summary>
        /// 在房间加载后获取敌人，添加到`aliveEnmList`中
        /// </summary>
        /// <param name="obj"></param>
            public void OnLoadRoomEvent()
            {
                StartCoroutine(OnLoadRoomEventIEnumerator());
            }

            IEnumerator OnLoadRoomEventIEnumerator()
            {
                var enms = FindObjectsByType<Enm>(FindObjectsInactive.Include,FindObjectsSortMode.None);
                //这段直接放进方法执行不用协程会导致游戏卡死
                while (enms.Length == 0)
                {
                    enms = FindObjectsByType<Enm>(FindObjectsInactive.Include,FindObjectsSortMode.None);
                    yield return null;
                }

                foreach (var item in enms)
                {
                    aliveEnmList.Add(item);
                }
            }
        ```

- 广播
    - CharacterBase

## 2.gameWinEvent,gameLoseEvent
- 监听
    - CardDeck
        - `ReleaseAllCards`
            - 对局结束后回收所有卡牌
            - 初始化`CardDeck`
- 广播
    - GameMgr

## 3.UIMgr
- 用于管理所有面板的启动
- 变量
    - 面板
        - `GameObject gamePlayPanel`
        - `GameObject gameWinPanel`
        - `GameObject gameLosePanel`
    - 
- 方法
```c#
/// <summary>
/// 加载房间时调用，监听端
/// </summary>
/// <param name="obj"></param>
    public void OnLoadRoomEvent(object obj)
    {
        Room curRoom = (Room)obj;

        switch (curRoom.data.type)
        {
            case RoomType.MinorEnm:
            case RoomType.EliteEnm:
            case RoomType.Boss:
                //出怪的房间打开对战界面
                gamePlayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.Restroom:
                break;
            default:
                break;
        }
    }

/// <summary>
/// 加载地图/菜单时调用
/// </summary>
    public void HideAllPanels()
    {
        gameLosePanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
    }

/// <summary>
/// 游戏胜利时调用
/// </summary>
    public void OnGameWinEvent()
    {
        gameLosePanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

/// <summary>
/// 游戏失败时调用
/// </summary>
    public void OnGameLoseEvent()
    {
        gameLosePanel.SetActive(false);
        gameLosePanel.SetActive(true);
    }
```

## 4.TurnBaseMgr控制player的启动
- 增加方法
```c#
/// <summary>
/// 房间加载后的事件
/// </summary>
    public void OnLoadRoomEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.data.type)
        {
            case RoomType.MinorEnm:
            case RoomType.EliteEnm:
            case RoomType.Boss:
                player.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                player.SetActive(false);
                break;
            case RoomType.Restroom:
                player.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 游戏结束返回地图的事件函数
    /// </summary>
    public void OnLoadMapEvent()
    {
        battleEnd = true;
        player.SetActive(false);
    }
```

- 增加监听`AfterLoadedRoomEvent`


[**回到目录索引**](#零索引)

---

# 二十七、抽卡逻辑

*在此之前已经做完了抽卡的面板以及卡的显示ui模板*

## 1.UI相关
```c#
    private void OnEnable() {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        container = rootElement.Q<VisualElement>("Container");

        for (int i = 0; i < count; i++)
        {
            //生成卡模板
            var card = cardTemplate.Instantiate();
            container.Add(card);
        }
    }

/// <summary>
/// 初始化卡模板里面的数据
/// </summary>
/// <param name="card"></param>
/// <param name="data"></param>
    void InitCard(VisualElement card, CardDataSO data)
    {
        cardData = data;

        var cardSpriteEle = card.Q<VisualElement>("CardSprite");
        var cost = card.Q<Label>("EnergyCost");
        var description = card.Q<Label>("CardDescription");
        var type = card.Q<Label>("CardType");

        cardSpriteEle.style.backgroundImage = new StyleBackground(data.sprite);
        cost.text = data.cost.ToString();
        description.text = data.description.ToString();
        type.text = data.type switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "技能",
            _ => throw new System.NotImplementedException(),
        };
    }
```

## 2.抽卡逻辑
- 哪里存着游戏里所有的卡牌呢？ => **CardMgr**
- 在`CardMgr`中
    - 增加变量
    - 增加新方法
        - `GetNewCardData`
            - 通过`do...while`保证前后两张卡不重复
            - ~~碎碎念：byd写的逻辑没问题但是测试的逻辑有问题，导致我在这白白耗了半个钟的美好时光TAT~~

## 3.绑定按钮

**所有的按钮绑定在OnEnable中完成**

- 卡牌的选择
    - 注册
        - `cardBtn.clicked += () => OnCardClicked(cardBtn,data);`
    - 方法实现
    ```c#
    /// <summary>
    /// 点击卡牌按钮后当前卡牌不可用
    /// </summary>
    /// <param name="cardBtn"></param>
    /// <param name="data"></param>
    private void OnCardClicked(Button cardBtn, CardDataSO data)
    {
        cardData = data;
        //Debug.Log(data.description);

        for (int i = 0; i < cardBtns.Count; i++)
        {
            if (cardBtns[i] == cardBtn)
            {
                cardBtns[i].SetEnabled(false);
            }
            else
            {
                cardBtns[i].SetEnabled(true);
            }
        }
    }
    ```
- 确认按钮
    - 注册`confirmBtn.clicked += OnConfirmBtnClicked;`
    - 方法实现
        - 逻辑：确认后把他加入玩家的卡牌库，有重复就增加数字
        - 在CardMgr中添加方法
            - `UnlockCard(CardDataSO newData)`
                - 如果包含该卡片则玩家卡牌库的此卡片数+1，不包含就加进去
        - 直接调用`CardMgr`的`UnlockCard(CardDataSO newData)`方法
        - 广播已经抽完卡了
            - *实际上我并没有添加新ObjectEventSO广播，因为我想让他抽完卡直接返回地图*

[**回到目录索引**](#零索引)

**自此，敌人、胜负与抽卡部分完成**

---

# 二十八、开始一局新的游戏

## 1.制作menu
```c#
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    VisualElement rootEle;
    Button newGameBtn,quitBtn;

    public ObjectEventSO newGameEvent;

    private void OnEnable() {
        rootEle = GetComponent<UIDocument>().rootVisualElement;
        newGameBtn = rootEle.Q<Button>("NewGameBtn");
        quitBtn = rootEle.Q<Button>("QuitBtn");

        newGameBtn.clicked += OnNewGameBtnClicked;
        quitBtn.clicked += OnQuitBtnClicked;
    }

    //unity游戏退出程序
    private void OnQuitBtnClicked() => Application.Quit();


    private void OnNewGameBtnClicked()
    {
        newGameEvent.RaiseEvent(null,this);
    }
}
```

## 2.开始一局新的游戏
1. 加载地图
2. 清除之前的地图缓存
- `GameMgr`增加方法监听
```c#
    public void OnNewGameEvent()
    {
        mapLayout.roomMapDataList.Clear();
        mapLayout.LinePosList.Clear();
    }
```

[**回到目录索引**](#零索引)

---

# 二十九、淡入淡出
## 1.淡入淡出
1. 创建UI Document
    - 很简单，就一块黑的背景
2. 控制淡入淡出的脚本
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
public class FadePanel : MonoBehaviour
{
    VisualElement bg;
    
    private void Awake() {
        bg = GetComponent<UIDocument>().rootVisualElement;
    }

    public void FadeIn(float duration)
    {
        DOVirtual.Float(0f,1f,duration,value => {
            bg.style.opacity = value;
        } ).SetEase(Ease.InQuad);
    }

    public void FadeOut(float duration)
    {
        DOVirtual.Float(1f,0f,duration,value => {
            bg.style.opacity = value;
        } ).SetEase(Ease.OutQuad);
    }
}
```

3. 在`SceneLoadManager`添加引用
- 遇到的问题：发现调用乱套了怎么办？
    - 使用`await Awaitable.WaitForSecondsAsync(float second)`方法
    - *需要unity6支持*

[**回到目录索引**](#零索引)

---

# 三十、打包游戏
## 1.关于`AddressableGroup`
- 正式打包请调整一下设置
    - `Play Mode Script`改为`Use Existing Build`

## 2.boot
需要一个boot场景才能在游戏一开始激活`Persistent`场景

因为A scene from the EditorBuildScenes list has been marked as addressable. 
It has thus been disabled in the build scenes list.

创建一个空的GameObject添加以下代码：
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoad : MonoBehaviour
{
    public AssetReference scene;
    private void Awake() {
        Addressables.LoadSceneAsync(scene);
    }
}
```

## 3.设置打包内容
- 在edit - project settings
    - Player那一栏可以改

## 4.遇到的问题
- 中文字体没打包进去

1. 添加一套汉字字库，比如阿里的普惠体，字体需要放到Resources/Font & Materials/文件夹下
2. 原始字库的Character需要改成Dynamic

3. 菜单中Window->Text->Font Asset Creator
Character Set要选中Custom Characters,然后把常用的汉字和字符复制到Custom Character List中

4. 点击Generater Font Atles生成纹理，然后保存
5. 在UIBuilder中选中需要改字体的元素，Font Asset选刚刚设置的字体就可以了。

## *5.常用汉字表2500字*
一乙二丁厂七卜人入八九十几儿了力乃刀又三于干亏士工土才寸下大丈与万上小口巾山千乞川亿个勺久凡及夕丸么广亡门义之尸弓己已子卫也女飞刃习叉马乡丰王井开夫天无元专云扎艺木五支厅不太犬区历尤友匹车巨牙屯比互切瓦止少日中冈贝内水见午牛手毛气升长仁什片仆化仇币仍仅斤爪反介父从今凶分乏公仓月氏勿欠风丹匀乌凤勾文六方火为斗忆订计户认心尺引丑巴孔队办以允予劝双书幻玉刊示末未击打巧正扑扒功扔去甘世古节本术可丙左厉右石布龙平灭轧东卡北占业旧帅归且旦目叶甲申叮电号田由史只央兄叼叫另叨叹四生失禾丘付仗代仙们仪白仔他斥瓜乎丛令用甩印乐句匆册犯外处冬鸟务包饥主市立闪兰半汁汇头汉宁穴它讨写让礼训必议讯记永司尼民出辽奶奴加召皮边发孕圣对台矛纠母幼丝式刑动扛寺吉扣考托老执巩圾扩扫地扬场耳共芒亚芝朽朴机权过臣再协西压厌在有百存而页匠夸夺灰达列死成夹轨邪划迈毕至此贞师尘尖劣光当早吐吓虫曲团同吊吃因吸吗屿帆岁回岂刚则肉网年朱先丢舌竹迁乔伟传乒乓休伍伏优伐延件任伤价份华仰仿伙伪自血向似后行舟全会杀合兆企众爷伞创肌朵杂危旬旨负各名多争色壮冲冰庄庆亦刘齐交次衣产决充妄闭问闯羊并关米灯州汗污江池汤忙兴宇守宅字安讲军许论农讽设访寻那迅尽导异孙阵阳收阶阴防奸如妇好她妈戏羽观欢买红纤级约纪驰巡画寿弄麦形进戒吞远违运扶抚坛技坏扰拒找批扯址走抄坝贡攻赤折抓扮抢孝均抛投坟抗坑坊抖护壳志扭块声把报却劫芽花芹芬苍芳严芦劳克苏杆杠杜材村杏极李杨求更束豆两丽医辰励否还歼来连步坚旱盯呈时吴助县里呆园旷围呀吨足邮男困吵串员听吩吹呜吧吼别岗帐财针钉告我乱利秃秀私每兵估体何但伸作伯伶佣低你住位伴身皂佛近彻役返余希坐谷妥含邻岔肝肚肠龟免狂犹角删条卵岛迎饭饮系言冻状亩况床库疗应冷这序辛弃冶忘闲间闷判灶灿弟汪沙汽沃泛沟没沈沉怀忧快完宋宏牢究穷灾良证启评补初社识诉诊词译君灵即层尿尾迟局改张忌际陆阿陈阻附妙妖妨努忍劲鸡驱纯纱纳纲驳纵纷纸纹纺驴纽奉玩环武青责现表规抹拢拔拣担坦押抽拐拖拍者顶拆拥抵拘势抱垃拉拦拌幸招坡披拨择抬其取苦若茂苹苗英范直茄茎茅林枝杯柜析板松枪构杰述枕丧或卧事刺枣雨卖矿码厕奔奇奋态欧垄妻轰顷转斩轮软到非叔肯齿些虎虏肾贤尚旺具果味昆国昌畅明易昂典固忠咐呼鸣咏呢岸岩帖罗帜岭凯败贩购图钓制知垂牧物乖刮秆和季委佳侍供使例版侄侦侧凭侨佩货依的迫质欣征往爬彼径所舍金命斧爸采受乳贪念贫肤肺肢肿胀朋股肥服胁周昏鱼兔狐忽狗备饰饱饲变京享店夜庙府底剂郊废净盲放刻育闸闹郑券卷单炒炊炕炎炉沫浅法泄河沾泪油泊沿泡注泻泳泥沸波泼泽治怖性怕怜怪学宝宗定宜审宙官空帘实试郎诗肩房诚衬衫视话诞询该详建肃录隶居届刷屈弦承孟孤陕降限妹姑姐姓始驾参艰线练组细驶织终驻驼绍经贯奏春帮珍玻毒型挂封持项垮挎城挠政赴赵挡挺括拴拾挑指垫挣挤拼挖按挥挪某甚革荐巷带草茧茶荒茫荡荣故胡南药标枯柄栋相查柏柳柱柿栏树要咸威歪研砖厘厚砌砍面耐耍牵残殃轻鸦皆背战点临览竖省削尝是盼眨哄显哑冒映星昨畏趴胃贵界虹虾蚁思蚂虽品咽骂哗咱响哈咬咳哪炭峡罚贱贴骨钞钟钢钥钩卸缸拜看矩怎牲选适秒香种秋科重复竿段便俩贷顺修保促侮俭俗俘信皇泉鬼侵追俊盾待律很须叙剑逃食盆胆胜胞胖脉勉狭狮独狡狱狠贸怨急饶蚀饺饼弯将奖哀亭亮度迹庭疮疯疫疤姿亲音帝施闻阀阁差养美姜叛送类迷前首逆总炼炸炮烂剃洁洪洒浇浊洞测洗活派洽染济洋洲浑浓津恒恢恰恼恨举觉宣室宫宪突穿窃客冠语扁袄祖神祝误诱说诵垦退既屋昼费陡眉孩除险院娃姥姨姻娇怒架贺盈勇怠柔垒绑绒结绕骄绘给络骆绝绞统耕耗艳泰珠班素蚕顽盏匪捞栽捕振载赶起盐捎捏埋捉捆捐损都哲逝捡换挽热恐壶挨耻耽恭莲莫荷获晋恶真框桂档桐株桥桃格校核样根索哥速逗栗配翅辱唇夏础破原套逐烈殊顾轿较顿毙致柴桌虑监紧党晒眠晓鸭晃晌晕蚊哨哭恩唤啊唉罢峰圆贼贿钱钳钻铁铃铅缺氧特牺造乘敌秤租积秧秩称秘透笔笑笋债借值倚倾倒倘俱倡候俯倍倦健臭射躬息徒徐舰舱般航途拿爹爱颂翁脆脂胸胳脏胶脑狸狼逢留皱饿恋桨浆衰高席准座脊症病疾疼疲效离唐资凉站剖竞部旁旅畜阅羞瓶拳粉料益兼烤烘烦烧烛烟递涛浙涝酒涉消浩海涂浴浮流润浪浸涨烫涌悟悄悔悦害宽家宵宴宾窄容宰案请朗诸读扇袜袖袍被祥课谁调冤谅谈谊剥恳展剧屑弱陵陶陷陪娱娘通能难预桑绢绣验继球理捧堵描域掩捷排掉堆推掀授教掏掠培接控探据掘职基著勒黄萌萝菌菜萄菊萍菠营械梦梢梅检梳梯桶救副票戚爽聋袭盛雪辅辆虚雀堂常匙晨睁眯眼悬野啦晚啄距跃略蛇累唱患唯崖崭崇圈铜铲银甜梨犁移笨笼笛符第敏做袋悠偿偶偷您售停偏假得衔盘船斜盒鸽悉欲彩领脚脖脸脱象够猜猪猎猫猛馅馆凑减毫麻痒痕廊康庸鹿盗章竟商族旋望率着盖粘粗粒断剪兽清添淋淹渠渐混渔淘液淡深婆梁渗情惜惭悼惧惕惊惨惯寇寄宿窑密谋谎祸谜逮敢屠弹随蛋隆隐婚婶颈绩绪续骑绳维绵绸绿琴斑替款堪搭塔越趁趋超提堤博揭喜插揪搜煮援裁搁搂搅握揉斯期欺联散惹葬葛董葡敬葱落朝辜葵棒棋植森椅椒棵棍棉棚棕惠惑逼厨厦硬确雁殖裂雄暂雅辈悲紫辉敞赏掌晴暑最量喷晶喇遇喊景践跌跑遗蛙蛛蜓喝喂喘喉幅帽赌赔黑铸铺链销锁锄锅锈锋锐短智毯鹅剩稍程稀税筐等筑策筛筒答筋筝傲傅牌堡集焦傍储奥街惩御循艇舒番释禽腊脾腔鲁猾猴然馋装蛮就痛童阔善羡普粪尊道曾焰港湖渣湿温渴滑湾渡游滋溉愤慌惰愧愉慨割寒富窜窝窗遍裕裤裙谢谣谦属屡强粥疏隔隙絮嫂登缎缓编骗缘瑞魂肆摄摸填搏塌鼓摆携搬摇搞塘摊蒜勤鹊蓝墓幕蓬蓄蒙蒸献禁楚想槐榆楼概赖酬感碍碑碎碰碗碌雷零雾雹输督龄鉴睛睡睬鄙愚暖盟歇暗照跨跳跪路跟遣蛾蜂嗓置罪罩错锡锣锤锦键锯矮辞稠愁筹签简毁舅鼠催傻像躲微愈遥腰腥腹腾腿触解酱痰廉新韵意粮数煎塑慈煤煌满漠源滤滥滔溪溜滚滨粱滩慎誉塞谨福群殿辟障嫌嫁叠缝缠静碧璃墙撇嘉摧截誓境摘摔聚蔽慕暮蔑模榴榜榨歌遭酷酿酸磁愿需弊裳颗嗽蜻蜡蝇蜘赚锹锻舞稳算箩管僚鼻魄貌膜膊膀鲜疑馒裹敲豪膏遮腐瘦辣竭端旗精歉熄熔漆漂漫滴演漏慢寨赛察蜜谱嫩翠熊凳骡缩慧撕撒趣趟撑播撞撤增聪鞋蕉蔬横槽樱橡飘醋醉震霉瞒题暴瞎影踢踏踩踪蝶蝴嘱墨镇靠稻黎稿稼箱箭篇僵躺僻德艘膝膛熟摩颜毅糊遵潜潮懂额慰劈操燕薯薪薄颠橘整融醒餐嘴蹄器赠默镜赞篮邀衡膨雕磨凝辨辩糖糕燃澡激懒壁避缴戴擦鞠藏霜霞瞧蹈螺穗繁辫赢糟糠燥臂翼骤鞭覆蹦镰翻鹰警攀蹲颤瓣爆疆壤耀躁嚼嚷籍魔灌蠢霸露囊罐

**自此，所有内容已完结，完结撒花*★,°*:.☆(￣▽￣)/$:*.°★* 。**

[**回到目录索引**](#零索引)