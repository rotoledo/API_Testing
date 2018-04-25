Feature: GoogleDistanceAPI
	In order to prepare myself for trips and commuting
	As a Google user
	I want to be told the distance and time travel between two addesses

Scenario Outline: Calculate successfully the distance and time travel between two addesses
	Given I have inserted <origin> into the attribute Origins
	And I have inserted <destination> into the attribute Destination
	And I have inserted <unit> into the attribute Units
	When I send a HTTP Get Request
	Then the response should be successful
	Then the response content should contain <origin> and <destination>
	Then the response should inform the estimated distance
	Then the response should inform the estimated time travel
Examples: 
| origin    | destination  | unit   |
| São Paulo | Porto Alegre | metric |