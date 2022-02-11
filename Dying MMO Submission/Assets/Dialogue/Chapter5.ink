//Asking Silk for Help
//Draft 1 | Text Updated 2/9 | Ink File Updated 2/10
VAR willowRelationship = 0
    //See if this can be connected to Unity/Previous Ink Files for continuity
VAR silkRelationship = 0
    //Same as for willowRelationship
VAR usualSpot = "Tower's Crest Viewpoint"
VAR sceneFinished = false
-> Start

===Start===
#TEAM CHAT OR SILK CHAT

//DEBUG SECTION, REMOVE LATER
-> Debug
= Debug
#silver
This is a debug question.
Have you been nice to me and Willow so far?
+   [Yes]
    ~ willowRelationship += 50
    ~ silkRelationship += 50
    #silver
    Yeah, I've been trying to be friendly to you weirdos.
    #silk
    Cool. Back to the story then.
+   [No]
    ~ willowRelationship -= 50
    ~ silkRelationship -= 50
    #silver
    No, you two are weird.
    #silk
    Rude, but okay. Back to the story then.
  
- 
#silver
So uh.
The Usual Spot?
Where is the Usual Spot?
#silk
Wouldn't you like to know?
#silver
Yes, that's why I'm asking!
You want me to pretend to be Sunny, you're going to need to help me here.
The  Usual Spot doesn't sound like something that someone just forgets.
#silk
Fight me.
#silver
Oh I'd love to.
#server
SILKENSCRAPS CHALLENGES YOU TO A DUEL
#silver
Oh.
{ //Conditional Split
    - silkRelationship + willowRelationship >= 2: //You've been nice
        -> Nice
    - else: //You've been rude
        -> Rude
}
= Nice
#silk
Sunny and I used to duel every now and then for fun.
Will you let me fight his account one more time?
Fight me, and I'll tell you where to go.
    -> Cont
= Rude
#silk
You've been a pain in the a\*s all day, and I want to blow off some steam.
Fight me, and I might tell you where to go.
    -> Cont

= Cont
#silver
Not giving me much of a choice here, are you.
+   [Agree to Fight]
    -> Fight
+   [Refuse to Fight]
    -> Refused

= Fight
~ silkRelationship += 10
#silver
Alright, you're on.
#silk
I know you're new to the game, but try to put up at least a little fight.

//Silk should talk during the fight, but it might be best to control that through Unity rather than Ink. I think it would be super cool to have text lines be responsive to things the player is doing, but implementation? :person_shrugging:
// For now, here are some lines to play in order.

#silk
Hah, nice dodge!
I hate that ability.
Come at me, Sunny!
Your gear's fallen behind buddy.
Nice try!
That's what you get for leaving us behind!
That's what you get for not saying goodbye!

//To control the outcome, we may need to divide this script into separate ones. For now, here's a debug choice.
#silk
Ughh, who won?
+   [I did]
    #silver
    Looks like I win.
    #silk
    Don't get used to it.
    #silver
    I won't? Game's ending today in case you forgot.
    #silk
    Oh, well, you know what I meant.
    Beginner's luck.
    #silver
    Whatever, I won.
    Tell me where the Usual Spot is so I don't make a fool of myself.
    #silk
    You'll find Willow at the {usualSpot}.
+   [You did]
    #silk
    Ha! I win again, Sunny.
    #silver
    It wasn't exactly a fair fight.
    Also.
    You've been calling me Sunny for a bit now.
    #silk
    Oh.
    Well.
    I mean.
    #silver
    Whatever, I lost.
    You just going to watch me make a fool of myself trying to find Willow before the game shuts down?
    #silk
    I said I'd tell you if you fought me, not if you won.
    They'll be at the {usualSpot}
-
#silk
Anyway, thanks for fighting me.
Playing against that account almost makes it feel like Sunny's still there.
Just a message away.
#silver
You're the one that basically forced me to duel you man.
#silk
I like you more when you don't talk.
We should get moving, Willow's waiting for us.
~ sceneFinished = true
-> END

= Refused
~ silkRelationship -= 10
#silver
Yeah, no, I'm not accepting that.
#silk
A\*****e
You're an insult to Sunny's memory in every way.
#silver
Look, I didn't sign up to deal with all his baggage.
And I didn't ask to be harassed by you all day.
#silk
Well now you're on your own.
Willow will be waiting for you.
Don't disappoint them.
#server
SILKENSCRAPS HAS LEFT THE PARTY
~ sceneFinished = true
-> END