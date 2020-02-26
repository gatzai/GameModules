##### 使用场景：View和Model的非耦合交互
##### 使用环境：
##### 涉及的设计模式：观察者模式，外观模式
##### 使用流程：
* 新建一个Facade对象
* 在Facade对象里注册Proxy，Mediator和Command对象
* 添加命令字符串，并在HandNotifiction中拦截和判断
