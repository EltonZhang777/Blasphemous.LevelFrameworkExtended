# Blasphemous Level Framework Extended

This context describes the additional level objects and editing behavior provided by the mod for Blasphemous 1.

## Level Objects

**Ground collectible**: A Level Framework `item-ground` object based on the game's `ACT_Collectible` component, collected by entering interaction range and pressing the usual interaction button. It grants its configured inventory reward once, then remains unavailable for collection.

**Blood-platform graph**：一种 `platform-blood-graph` LevelEdit addition，用节点、可重复的 initial 节点和显式 `-->` 有向边定义一条由普通 `FaithPlatform` 实例组成的路线。节点 transform 是最终绝对值；`ObjectData.id` 只作为诊断 batch 标签。
