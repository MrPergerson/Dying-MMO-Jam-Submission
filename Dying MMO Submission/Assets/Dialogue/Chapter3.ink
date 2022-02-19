INCLUDE GlobalVariables.ink

//Meeting with Willow
//Draft 1.2 | Text Updated 2/18 | Ink File Updated 2/18
VAR dungeonDifficulty = 0
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR sceneFinished = false
-> Start

===Start===
#tab: Willow #w1ll0w_w1sp
There you are, accept my invite.
#tab: Party #w1ll0w_w1sp
Are you familiar with the idea of a bucket list?
+   [Yeah]
    #s1lverSun
    Yeah.
+   [No]
    #s1lverSun
    No.
    #w1ll0w_w1sp
    It's a list of the things you want to do before you kick the bucket.
-
#w1ll0w_w1sp
We should make one for today!
Obviously it's the game dying.
Not us.
But anyway, last day in the game!
How do you want to spend it?
I have a couple of ideas, but you're the one who's been gone for months, so you choose.
#s1lverSun
I wanted to fight stuff.
#w1ll0w_w1sp
Right on! Violence! Yeah!
Let's end this game with a bang.
Ooh, let's do Throne of the Wayward King.
I'm going to miss that dungeon. Hope we can find something like it in some other game.
Oh hey, Silk's online too!
I'll invite them!
#s1lverSun
Er, yeah, go for it.
#[SERVER]
silkenscraps HAS JOINED THE PARTY
#silkenscraps
Hey gamers.
#w1ll0w_w1sp
Sup nerd.
Did you see who's joining us? It's Sunny!
#silkenscraps
Yeah, the prodigal sun returns.
#w1ll0w_w1sp
Oh my god you did not.
+   [Join In]
    ~ willowRelationship++
    ~ silkRelationship++
    #s1lverSun
    That was pretty good.
    You could even say it was...
    #w1ll0w_w1sp
    No! Don't you dare!
    #s1lverSun
    ...smooth as silk?
    #w1ll0w_w1sp
    Aaaauuughghgh.
+   [Say Nothing]
-
#w1ll0w_w1sp
Alright, now that the puns are out of our system, let's get to it!
I'll meet you two there.
#tab: Silk #s1lverSun
Hey, how do I join a dungeon?
#silkenscraps
God, you're hopeless.
Dungeon Portal's to the west.
Figure the rest out yourself.
#s1lverSun
Gee, thanks.
~sceneFinished = true
~ scene03Finished = true
-> END