# RI_QuestPlayer
 360 Video Player for Quest and Projector
 
## The scene used for Quest App

It's called QuestScene, in Assets/Scenes folder.

## How to update video

1. Place 360 video in Assets/360Videos folder.
2. Drag it to the Video Clip in VideoPlayer gameObject in QuestScene
3. In QuestScene, go to MQTT===ChangeHere gameObject, find Video Controller script attached to it
4. Update the starts and ends based on the new video:
   The 
