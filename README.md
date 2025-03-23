# WordNotes 单词便签
> 旨在用户浏览其他内容时，能时不时瞟一眼标签上的单词，以加深记忆

最新版本：v0.3.2

github: https://github.com/IlightnNG/WordNotes

---
## 使用说明
### 1. 主窗口
主窗口会**随机**播放词库中的单词。

显示分为两种模式：

1. 随机模式
2. 每日单词模式

#### 1.随机模式
随机从词库中播放单词

#### 2.每日单词模式
显示的单词分为*今日新词*和*今日复习词*两组。以下介绍中最后一次启动软件时的那一天称为*昨日*。

今日新词：昨日未出现的单词

今日复习词：从昨日的新词和复习词中随机选择

今日新词和复习词的数量以及播放时间间隔可在设置中调节。主窗口可按住鼠标左键拖动。点击英文单词可跳转有道词典。

#### 上方按钮功能（从左到右）：
1. 暂停播放按钮：暂停播放下一个单词
2. 五角星按钮：收藏该单词，**收藏后的单词后续出现概率增加**，可在菜单-收藏单词页面统一管理
3. 箭头按钮：显示下一个单词
4. 菜单按钮：打开菜单窗口
5. 关闭按钮：关闭程序

#### 右下角折角标签按钮功能：
1. 点击按钮，将当前展示的单词以便签形式贴在屏幕上
2. 便签默认置顶，可拖动和关闭

---
### 2.菜单窗口

显示包括列表和设置等额外信息的窗口。上方橙色区域可按住鼠标左键拖动。

#### 上方导航栏功能（从左到右）：
1. 历史单词页面：显示今日新词和复习词数以及最近展示的单词列表，点击英文单词可跳转浏览器搜索，右侧五角星按钮可收藏
2. 收藏单词页面：显示所有收藏的单词，五角星按钮可取消收藏
3. 设置页面：可更改单词播放时间间隔，词库路径等

#### 标签行按钮功能
1. 刷新按钮：重新加载当前页面
2. 关闭按钮：关闭菜单窗口

---
### 3.设置窗口
设置文件存放在软件根目录的Config文件夹中

可更改设置如下
1. 播放时间间隔
2. 收藏单词出现概率
3. 每日新词和复习词数
4. 词库路径

---
## 更新日志

### v0.3.2
1. 将主窗口搜索按钮改为暂停播放按钮
3. 主窗口可点击英文单词实现搜索功能，这与其他窗口一样

### v0.3.1
1. 新增随机模式和每日单词模式切换
2. 修复了时间间隔改变后会生成多个timer的bug
3. 修复了复习单词序列来源混乱bug
4. 优化了历史单词页面的复习单词总数显示

### v0.3
1. 新增今日新词和复习词系统，历史单词页面可见今日词数
2. 设置页面新增设置“今日新词和复习词数量”，“收藏单词出现概率”
3. 优化了随机单词生成系统和设置页面后端

### v0.2
1. 新增设置页面，实现用户设置持久化，用户可调节单词播放时间间隔和词库路径，支持恢复默认设置
2. 优化主窗口ui，新增关闭按钮
3. 优化菜单窗口ui，将导航栏移动至顶部
4. 菜单页面新增点击单词可直接跳转搜索功能，新增收藏按钮
5. 优化主窗口折角按钮动画，并调整便签窗口出现位置至主窗口附近

### v0.1
1. 完成基础主窗口，菜单窗口，便签窗口ui
2. 完成从词库随机展示英文单词及其中文解释这核心功能
3. 完成搜索，收藏，下一单词，菜单按钮功能
4. 菜单窗口实现显示历史单词列表和收藏单词列表，以及刷新和关闭功能