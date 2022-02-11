//After the Bucket List
//Draft 1 | Text Updated 2/8 | Ink File Updated 2/10
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR willowSuspicion = 0
    //Same as for willowRelationship
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR sceneFinished = false
-> Start

===Start===
#TAKES PLACE IN TEAM CHAT TAB
#willow
My god, you're rusty.
Hey, I'm going to need to leave the party for a bit.
There's some unifnished business I need to see to alone.
I'll be back later though!
We should spend the end of the game together.
Let's meet up at the Usual Spot when the game is about to end.
Sound good?
#silk
Sure.
+   [Confirm]
    #silver
    Alright.
+   [Usual Spot?]
    ~ willowSuspicion++
    #silver
    The Usual Spot?
    #willow
    Haha, good one Sunny.
-
#willow
You two play nice now.
See you then!
#server
W1LL0W_W1SP HAS LEFT THE PARTY
~ sceneFinished = true
-> END