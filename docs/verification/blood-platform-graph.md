# Blood-Platform Graph 手工验证

适用范围：#35 `platform-blood-graph`。

已执行的烟测场景位于 `test/blood-platform-graph-d20z02s01/D20Z02S01.json`，使用
`D20Z02S01`、坐标域 `x = [515, 565]`、`y = [-160, -140]`。

## 前置

1. 在 `Modding/levels/Level Framework Extended/<Scene>.json` 中，把 graph addition 放在普通 `platform-blood` addition 之前。
2. 使用以下声明（按实际场景坐标调整）：

```json
{
  "type": "platform-blood-graph",
  "id": "route-check",
  "properties": [
    "node = entry-a; position = [0, 10, 0]; scale = [1, 1, 1]",
    "node = entry-b; position = [4, 12, 0]",
    "node = branch; position = [8, 10, 0]; rotation = [0, 0, 15]",
    "node = merge; position = [12, 12, 0]; scale = [1.2, 0.8, 1]",
    "initial = entry-a",
    "initial = entry-b",
    "edge = entry-a --> branch",
    "edge = entry-b --> branch",
    "edge = branch --> merge",
    "edge = merge --> entry-a"
  ]
}
```

3. 再加一个普通平台，属性为 `false`、`merge`（graph 在前），验证普通平台可以 eagerly 指向 graph 节点。

## 观察项

1. 进入场景后，`entry-a`、`entry-b` 保持 original first-platform 行为；触发 `REVEAL_FAITH_PLATFORMS` 时两者都能独立显示。
2. 分别踩入两个 entry，各自只显示 `branch`；踩入 `branch` 只显示 `merge`；踩入 `merge` 只显示 `entry-a`，不因汇聚或环产生额外反向/跨边显示。
3. `branch` 的 15° Z 旋转、`merge` 的 `[1.2, 0.8, 1]` scale 只作用于各自节点；graph addition 的 transform 不叠加。
4. 退出平台后按原 deactivation 延迟隐藏；粒子流只沿声明方向。
5. 普通平台踩下后按原有 `platform-blood` 行为显示 `merge`。
6. 控制台无 `platform-blood-graph` 错误；故意制造未定义边或不可达节点时，整批不生成平台，并出现带 batch label/节点上下文的错误。

## 已知边界

- 普通平台指向 graph 节点要求 graph 在最终 merged `additions` 中更早出现；反向顺序属于 #36，不在 #35 范围。
- 场景范围 ID 唯一性属于 #37，不在 #35 范围。

## 已执行结果

2026-11-12 在真实 Modding profile 中执行 `D20Z02S01` 烟测：

- 启动状态为 `mod_loaded`，日志确认 `platform-blood-graph` 注册成功，
  `D20Z02S01` 的 LevelEdit 成功应用。
- 玩家观察到 4 个 graph 血平台的大小、位置、旋转全部正确。
- 玩家观察到 graph 连通关系正确，无可见异常或崩溃。
