##### 使用引擎：Unity
##### 主要功能：管理一系列UI的显示，关闭，弹出等，所以说只管理UI的显示逻辑
##### 主文件：UIManager
##### 流程：
* 首先建立要打开的UIPrefab（命名：xxxMenu，路径要注意）
* 其次此Prefab的操作脚本（命名：xxxUI）
* 最后在UIManager的Awake中添加新UI（UIPath?.Add("xxxUI", "UIPrefabs/xxxMenu");）
