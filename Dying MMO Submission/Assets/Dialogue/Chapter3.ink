//Meeting with Willow
//Draft 1.1 | Text Updated 2/9 | Ink File Updated 2/13
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
I'd love to revisit some older levels.
Or maybe take on a dungeon raid.
Oooh, or we could go to the city and hang out with other players.
What do you think?
//  [Explore the World]
+   [Take on a Dungeon]
    #s1lverSun
    I wanted to fight stuff.
    #w1ll0w_w1sp
    Right on! Violence! Yeah!
    Let's end the game with a bang.
    Need a warm up? Or want to go right into the hard stuff?
    ++  [Easy]
        #s1lverSun
        Let's take it easy.
        #w1ll0w_w1sp
        Alright, I'll queue us up for an easy Dungeon.
        ~ dungeonDifficulty = 1
    ++  [Hard]
        #s1lverSun
        Let's take on the end-game content.
        I came here for a challenge.
        #w1ll0w_w1sp
        That's what I like to hear!
        Queuing us up for the big one.
        ~ dungeonDifficulty = 2
//  [Socialize]
-
#w1ll0w_w1sp
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
~sceneFinished = true
-> END