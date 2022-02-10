//Meeting with Willow
//Draft 1.1 | Text Updated 2/9 | Ink File Updated 2/10
VAR dungeonDifficulty = 0
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR sceneFinished = false
-> Start

===Start===
#willow
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
#willow
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
    #silver
    I wanted to fight stuff.
    #willow
    Right on! Violence! Yeah!
    Let's end the game with a bang.
    Need a warm up? Or want to go right into the hard stuff?
    ++  [Easy]
        #silver
        Let's take it easy.
        #willow
        Alright, I'll queue us up for an easy Dungeon.
        ~ dungeonDifficulty = 1
    ++  [Hard]
        #silver
        Let's take on the end-game content.
        I came here for a challenge.
        #willow
        That's what I like to hear!
        Queuing us up for the big one.
        ~ dungeonDifficulty = 2
//  [Socialize]
-
#willow
Oh hey, Silk's online too!
I'll invite them!
#silver
Er, yeah, go for it.
#SWITCH TO TEAM CHAT TAB
#server
SILKENSCRAPS HAS JOINED THE PARTY
#silk
Hey gamers.
#willow
Sup nerd.
Did you see who's joining us? It's Sunny!
#silk
Yeah, the prodigal sun returns.
#willow
Oh my god you did not.
+   [Join In]
    ~ willowRelationship++
    ~ silkRelationship++
    #silver
    That was pretty good.
    You could even say it was...
    #willow
    No! Don't you dare!
    #silver
    ...smooth as silk?
    #willow
    Aaaauuughghgh.
+   [Say Nothing]
-
#willow
Alright, now that the puns are out of our system, let's get to it!
~sceneFinished = true
-> END