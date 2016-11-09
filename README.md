Procedural Terrain Generator
----------------------------
A node based editing tool to generate Procedural Terrain

Usage Description:
------------------
1) Download the PTG_NodeEditor unity package file from the repository

2) Create a new project in unity & import the downloaded package

3) Create a Plane in unity by selecting GameObject > 3D Object > Plane from the top Menu Bar

4) Create a new material and apply a suitable color or texture to it & apply it to the plane

5) Select Procedural Terrain Generator from the top Menu Bar & select Launch Node Editor

6) Right Click inside grid & select Create Graph & give it a name

7) Right Click & Select Map Input Node. Enter dimension for map e.g. 100 for width & 100 for height

8) Right Click & add Perlin Adjust Node and Map Output Node. Connect Map Input to Perlin by left clicking on the Map Input Node outlet to the inlet of Perlin Node. Connect the Perlin Node to Map Output Node the same way.

9) Enter all parameters for Perlin Node. e.g. Noise Scale  - 27.6, Octaves - 4, Persistance - Drag Slider halfway i.e. 0.5, Lacunarity - 2.71, Seed - 0, OffsetX - 0, OffsetY - 0

10) The Map Output Node updates to display the noisemap being generated. Play around with the Perlin Values untill you get the desired output

11) Select Map Output Node. On the Property View to the right, an enlarged noisemap preview is displayed. 

12) Click on the handle below where it says "Select Plane" to open the Asset Selector. Go to the Scene Tab and select the plane created previously to generate the terrain. 

13) Adjust the height multiplier in the output node property view to adjust the height of the terrain as required.

14) You can adjust the noise parameters & apply the changes by pressing the "Generate terrain" button in the Map Output Node Property view.

15) You can test the terrain by importing & dropping a FirstPersonController prefab above the terrain from the Characters Unity Standard Package & pressing play.

Alternatively, you candownload the whole project to play the demo GameScene that has been setup.

Current Bugs:
------------
1) When loading a graph, sometime the last selected node moves out of Node Graph viewable area making it unable to reselect the node or edit its parameters.
Workaround : In order to ensure you save the created NodeGraph properly ensure that no nodes are selected before closing the node editor or unloading the graph.

2) If you try to connect all three nodes without having any parameters set on the Map Input node unity crashes.
Workaround : Ensure width & height are specified before connecting Map Input Node to Perlin Node and Perlin Node to Map Output Node.

2) Unity crashes if you try to connect the Map Input Node directly to Output Node
Workaround : Always ensure Map Input Node first connects to Perlin Node and then connect the Perlin Node to Output Node.


Link to detailed tool description with screenshots on my blog: 
--------------------------------------------------------------
http://paragpadubidri.com/portfolio/single-post/2016/10/09/Procedural-Terrain-Generation-using-Node-Based-Editor


References:
-----------
Landmass Generation tutorials by Sebastian Lague - https://www.youtube.com/user/Cercopithecan/
Basic Node based editor system by GameTutor - http://www.gametutor.com/live/tutorials/unity/creating-a-node-based-editor/
Unity Forums on Node based editing systems - https://forum.unity3d.com/threads/simple-node-editor.189230/
