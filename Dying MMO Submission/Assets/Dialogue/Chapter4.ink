//After the Bucket List
//Draft 1 | Text Updated 2/8 | Ink File Updated 2/13
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR willowSuspicion = 0
    //Same as for willowRelationship
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR sceneFinished = false
-> Start

===Start===
#tab: Party #w1ll0w_w1sp
My god, you're rusty.
Hey, I'm going to need to leave the party for a bit.
There's some unifnished business I need to see to alone.
I'll be back later though!
We should spend the end of the game together.
Let's meet up at the Usual Spot when the game is about to end.
Sound good?
#silkenscraps
Sure.
+   [Confirm]
    #s1lverSun
    Alright.
+   [Usual Spot?]
    ~ willowSuspicion++
    #s1lverSun
    The Usual Spot?
    #w1ll0w_w1sp
    Haha, good one Sunny.
-
#w1ll0w_w1sp
You two play nice now.
See you then!
#[SERVER]
w1ll0w_w1sp HAS LEFT THE PARTY
~ sceneFinished = true
-> END