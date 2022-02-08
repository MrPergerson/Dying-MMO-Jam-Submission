-> Conversation1
//This test ink is meant to demonstrate a method for naming characters I've previously worked with as well as using Ink's tools to have something of a branching path with consequences.
=== Conversation1 ===
#CharacterC
Hey.
Who are you?
#CharacterA
*   What do you mean?
*   [Say Nothing]
    #CharacterC
    Hello? I'm talking to you!

- #CharacterC
You're full of shit.
I know you're not the real CharacterD.

#CharacterA
*   [Tell the truth]
    -> truth
*   [Lie]
    -> lie

= truth
    #CharacterA
    Alright, you got me, I'm not them.
    I'm using a stolen account.
    -> cont
= lie
    #CharacterA
    I don't know what you're talking about man, how could I not be me?
    -> cont

= cont
#CharacterC
I don't like you.
{Conversation1.truth: But at least you've told the truth.}
{Conversation1.lie: You don't know when to call it quits, do you.}
-> END