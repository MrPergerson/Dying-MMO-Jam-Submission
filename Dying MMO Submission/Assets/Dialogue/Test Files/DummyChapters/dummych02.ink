INCLUDE dummyGlobals.ink

VAR silkRelationship = 0
    //By the end of the scene,
    //Maximum Relationship = 4
    //Minimum Relationship = -2
VAR sceneFinished = false
-> Start

===Start===
#tab: Silk #silk
~ currentTab = 3
Who are you?
+   [Respond]
    ~ silkRelationship--
    #silver
    What do you mean?
    #silk
    Don't give me that.
    You're full of s\**t
+   [Ignore]
    ~ silkRelationship -= 2
    #silk
    Answer me, g\*******t!
-
#silk
I know you aren't Sunny.
#silver
uhhh
~ sceneFinished = true
~ scene02Finished = true
-> END