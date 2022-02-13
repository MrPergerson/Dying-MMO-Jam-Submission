INCLUDE dummyGlobals.ink

VAR dungeonDifficulty = 0
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR sceneFinished = false
-> Start

===Start===
#willow
~ currentTab = 2
There you are, accept my invite.
Are you familiar with the idea of a bucket list?
+   [Yeah]
    #silver
    Yeah.
+   [No]
    #silver
    No.
    #willow
    It's a list of the things you want to do before you kick the bucket.
-
#silver
Cool
~sceneFinished = true
~scene03Finished = true
-> END