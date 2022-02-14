INCLUDE GlobalVariables.ink

//Lose the Duel
//Draft 1 | Text Updated 2/9 | Ink File Updated 2/13
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR usualSpot = "Tower's Crest Viewpoint"
VAR sceneFinished = false
-> Start

===Start===
#tab: Party #silkenscraps
Ha! I win again, Sunny.
#s1lverSun
It wasn't exactly a fair fight.
Also.
You've been calling me Sunny for a bit now.
#silkenscraps
Oh.
Well.
I mean.
#s1lverSun
Whatever, I lost.
You just going to watch me make a fool of myself trying to find Willow before the game shuts down?
#silkenscraps
I said I'd tell you if you fought me, not if you won.
They'll be at the {usualSpot}
Anyway, thanks for fighting me.
Playing against that account almost makes it feel like Sunny's still there.
Just a message away.
#s1lverSun
You're the one that basically forced me to duel you man.
#silkenscraps
I like you more when you don't talk.
We should get moving, Willow's waiting for us.
~ sceneFinished = true
~ scene05LoseFinished = true
-> END