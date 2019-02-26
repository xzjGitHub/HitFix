---
---Lua 脚本启动ab
--- Created by Yoki.
--- DateTime: 2019/2/11 13:54
---

local string loc_sceneName=nil
local string loc_abName=nil
local string loc_assetName=nil

--导入xlua包

local util=require("xlua.util")

--实例化ab核心管理类
local abManager=CS.AssetBundleMgr
local abObj=abManager.Instance

--加载ab包
local function LoadAbPackage(sceneName,abName,assetName)

end


--回调方法
local function FinishWork()

end