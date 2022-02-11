INCLUDE dummyGlobals.ink

VAR willowRelationship = 0
    //Max = 2
    //Min = -2
VAR willowSuspicion = 0
    //Max = 2
    //Min = 0
VAR sceneFinished = false
-> Start

===Start===
#willow
Yoooooo, is that you Sunny?
I haven't seen you online in ages! How have you been?
+   [Play it off]
    ~ willowRelationship++
    #silver
    I've been good, you?
    #willow
    Same old, same old.
+   [Express Confusion]
    ~ willowRelationship--
    ~ willowSuspicion++
    #silver
    Who are you again?
    #willow
    Ouch man, it hasn't been that long I hope.
    It's me, Willow. We played every Friday night?
    #silver
    Oh, right, sorry. It's been a while.
-
#willow
Alright! Meet me at {meetingPlace == "": ... | {meetingPlace}!} when you're ready and we'll get this party started!

#silver
Sounds good!

~ sceneFinished = true
~ scene01Finished = true
-> END