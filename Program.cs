/*
 * Project name: BowlingGame
 * Author: Colby Schaeding
 * Date: 3/29/2022
 * 
 * Description: This program is meant to imitate a randomized game
 *              of bowling. Each shot 'thrown' will knock down a
 *              random number of pins between 0 and 10. 10 pins
 *              shot down in one ball is a strike, and will earn
 *              a massive bonus in scoring based on how well the
 *              next two ball rolls perform. 10 pins knocked down
 *              using two throws during a single 'frame' results in
 *              a 'spare,' earning a minor scoring boost based on 
 *              the very next ball roll. If no strikes or spares are
 *              achieved, scoring is added based on the number of 
 *              pins knocked down in that frame with no added bonus.
 *              The final score is displayed at the end after the 
 *              program finishes.
 *              
 * Extra Notes: -This program was built for a coding challenge presented
 *              by Venminder Inc, as a part of their interview process.
 *              -Coded in C# using Visual Studio
 */

public class BowlingGame
{

    // Declare variables

    public static int randomRoll1;
    public static int randomRoll2;
    public static int randomRoll3;

    public static int currentRoll;
    public static int remainingPins;

    public static int frame;

    // used for calculating the final score
    public static int score;
    public static int temp;

    /*
     * This method is the primary method called for this program.
     * A ball will be thrown with each call to this method. The
     * first ball will be shot as normal. Depending on how well the
     * first ball's shot was, the second ball will either not be
     * recorded (due to the first ball being a strike before the 
     * final frame), thrown for a chance to grab a spare, or thrown
     * as another chance for a strike on the final frame. The third
     * throw only occurs on the final frame if one of the other two
     * throws in that same frame achieved either a strike or spare.
     */
    public static void RandomRoll(int frame)
    {
        Random rng = new Random();
        

        if (currentRoll == 1)
        {
            remainingPins = (10 - rng.Next(1, 11));
            randomRoll1 = (10 - remainingPins);
            currentRoll++;
        }
        else if (currentRoll == 2)
        {
            if (randomRoll1 == 10 && frame != 9)
            {
                randomRoll2 = 0;
                remainingPins = 0;
            }
            else if (frame == 9)
            {
                if (randomRoll1 == 10 || remainingPins == 0)
                {
                    remainingPins = 0;
                    remainingPins = (10 - rng.Next(1, 11));
                    randomRoll2 = (10 - remainingPins);
                    currentRoll++;
                }
                else
                {
                    randomRoll2 = (rng.Next(1, remainingPins));
                }
            }
            else
            {
                randomRoll2 = (rng.Next(1, remainingPins + 1));
            }
        }
        else if (currentRoll == 3)
        {
            //determine whether this shot will start with 10 pins or 
            //continue off of roll #2's remaining pin count
            if ((randomRoll2 == 10) || (randomRoll1 + randomRoll2 == 10)) // all pins will be set up
            {
                remainingPins = (10 - rng.Next(1, 11));
                randomRoll3 = (10 - remainingPins);
            }
            else
            {
                randomRoll3 = (rng.Next(1, remainingPins + 1));
            }

        }
    }

    /* 
     * This method will format the end results screen, showcasing
     * all the rolls the player got for each frame, and what their
     * final score is.
     */
    private static void FormatScore(Frame[] myFrames, int score)
    {
        Console.WriteLine("Frame " + " Roll 1 " + " Roll 2 " + " Roll 3 ");
        for (int i = 0; i < 10; i++)
        {
            if (i < 9)
                Console.WriteLine((i+1) + "\t" + (myFrames[i].roll1) + "\t" + (myFrames[i].roll2) + "\t");
            else if (i == 9)
                Console.WriteLine((i + 1) + "\t" + (myFrames[i].roll1) + "\t" + (myFrames[i].roll2) + "\t" + (myFrames[i].roll3));
        }
        Console.WriteLine("Final Score: " + score);
    }

    /*
     * This method will calculate the score based on the rolls
     * given by the random number generator. A strike will add
     * 10 + the next two roll values, a spare will add 10 + the
     * next roll value only, otherwise the two rolls in a given
     * frame will just be added to the score with no bonus.
     */
    private static int CalcScore(Frame[] myFrames)
    {
        int score = 0;
        int temp = 0;

        // loop to perform score calculation after rolls are completed
        for (int i = 0; i < 9; i++)
        {
            temp = 0;
            // if roll 1 was a strike, add 10 + the following two rolls to the score
            if (myFrames[i].roll1 == 10)
            {
                temp = (10 + myFrames[i + 1].roll1);

                if (myFrames[i + 1].roll1 == 10)
                    temp += (myFrames[i + 2].roll1);
                else
                    temp += (myFrames[i + 1].roll2);

                score += temp;
            }

            // if roll 2 was a spare, add 10 + the following roll to the score
            else if (myFrames[i].roll1 + myFrames[i].roll2 == 10 && frame != 9)
            {
                temp = (10 + myFrames[i + 1].roll1);
                score += temp;
            }
            // else (none of the above)
            else
                score += (myFrames[i].roll1 + myFrames[i].roll2);
        }

        // add score resulting from the 10th and final frame
        if (myFrames[9].roll1 == 10)
            score += (10 + myFrames[9].roll2 + myFrames[9].roll3);
        if (myFrames[9].roll2 == 10)
            score += (10 + myFrames[9].roll3);

        score += (myFrames[9].roll1 + myFrames[9].roll2 + myFrames[9].roll3);
        
        return score;
    }

    /*
     * This method initializes a few of the starting variables
     */
    private static void Init()
    {
        frame = 1;
        remainingPins = 10;
        currentRoll = 1;
    }

    public static void Main(string[] args)
    {
        Init();
        Frame[] myFrames = new Frame[10];
        
        // loop to perform random rolls on each frame,
        // calling the RandomRoll() function each time
        // the 'ball' is thrown. Each roll is stored in
        // a Frame object array to be referenced later.
        for (int frame = 0; frame < 10; frame++)
        {
            currentRoll = 1;
            RandomRoll(frame);
            RandomRoll(frame);

            // store each roll's value for future calculations
            myFrames[frame] = new Frame(randomRoll1, randomRoll2);

            // if it is the final frame and there has been either 
            // a strike or a spare in the last two throws, roll
            // a third bowl for this frame only.
            if (frame == 9 && (randomRoll1 + randomRoll2 >= 10))
            {
                currentRoll++;
                RandomRoll(frame);
                myFrames[frame].SetRoll3(randomRoll3);
            }
        }

        // call the CalcScore() and FormatScore() methods
        score = CalcScore(myFrames);
        FormatScore(myFrames, score);
    }
}