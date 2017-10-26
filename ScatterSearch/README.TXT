This is a Scatter Search algorithm that is applied to the problem of finding a secret phrase
Things to improve:
1. When the reference set stops updating, then we need to generate new Initial Solutions and add new initial solutions to reference set for diversification
2. Make sure that when crossing that  all letters are present in the subset and not being excluded
    Right now, when crossing, the set loses some of letters which makes it impossible to find final solution
4. Improvement function that I have is very bad, does not really improve; 
	- For example add a template, and if the letter is correct, then keep it in that spot for the complete reference; 
	- Perhaps after crossing, make sure all letters are in the set, and if not, add correct number of letters back to the set

	Not this algorithm does not provide solution to the problem, but comes very close to it. 
	In order to get the solution, the improvements above have to be made

Notes: 2017-10-25
1. updated crossover function: now it is not replacing chromosome of the parentA if it matches secret phrase 
2. updated improvement function: it now re-shuffles spaces to match the phrase spaces. 
Result: now it finds the secret phrase 99% of the time. Before it was getting close. 

Observation: Improvement function, and selective crossover is extreamly important component of Scatter search. 
