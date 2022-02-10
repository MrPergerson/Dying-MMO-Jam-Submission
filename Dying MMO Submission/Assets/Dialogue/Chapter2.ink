//Silk Messages Silver
//Draft 1 | Text Updated 2/7 | Ink File Updated 2/10
VAR silkRelationship = 0
    //By the end of the scene,
    //Maximum Relationship = 4
    //Minimum Relationship = -2
VAR sceneFinished = false
-> Start

===Start===
#silk
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
So who are you and what the hell are you doing on his account?
+   [Tell the Truth]
    ~ silkRelationship++
    #silver
    I bought the account online.
    It was hacked and put on discount since the game is ending today.
    ++ [Apologize]
        ~ silkRelationship++
        I'm sorry.
        I didn't expect the original owner to have active friends online.
    ++ [Say Nothing]
    --
    #silk
    I don't like you.
    But at least you had the decency to tell the truth.
+   [Lie]
    ~ silkRelationship--
    #silver
    I don't know what you're talking about.
    How could I not be me?
    #silk
    I don't like you.
    Really trying to be a piece of s\**t here aren't you.
-
#silk
I knew Sunny in real life.
He died a few months ago.
+   [Give Condolences]
    ~ silkRelationship += 2
    #silver
    Sheesh, I'm sorry to hear that.
    #silk
    I don't want to hear it from you, hacker.
+   [Say Nothing]
-
#silk
I want to report your a\*s so bad.
But what's the point on the last day of the game.
#silver
Thanks I guess?
#silk
Shut it.
I don't know what would be worse, banning Sunny's account or letting some rando run wild on it.
#silver
If you're going to spend all day on it, I've got somewhere to be.
Someone else invited me to a party.
#silk
What?
Who?
#silver
Someone called w1ll0w_w1sp.
#silk
S\**t. No one told them.
Look, rando, you stole my friend's account.
But if you have any decency at all, will you do me a favor?
Play along with Willow, but don't tell them about Sunny yet.
Help them have a good last day. I'll tell them when the time is right.
Willow means a lot to both of us. They deserve to have something to remember him by.
#silver
Sounds a little dishonest.
#silk
LIKE YOU CAN TALK!
+   [Agree to Help]
    ~ silkRelationship++
    #silver
    I'll do it.
    Beats getting harassed by you all day.
    #silk
    Oh I'll be keeping an eye on you.
    You better not try anything.
    But thank you.
    I'll check in later.
-
~ sceneFinished = true
-> END