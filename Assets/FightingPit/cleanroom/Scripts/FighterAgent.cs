using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityStandardAssets.CrossPlatformInput;

namespace FightingPit
{
	public enum Team
	{
	    Blue = 0,
	    Red = 1
	}

	[RequireComponent(typeof (FighterCharacter))]
	public class FighterAgent : Agent
	{
	    [HideInInspector]
	    public Team team;
	    bool crouch = false;
	    bool m_Jump = false;
	    //public GameObject MyKey; //my key gameobject. will be enabled when key picked up.
	    public bool IHaveAKey; //have i picked up a key
	    private FighterSettings m_PushBlockSettings;
	    BehaviorParameters m_BehaviorParameters;
	    private Rigidbody m_AgentRb;
	    private FighterCharacter m_Character;
	    private FightingPitEnvController m_GameController;
	    Animator m_Animator;
	    

	    public override void Initialize()
	    {
		m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
		if (m_BehaviorParameters.TeamId == (int)Team.Blue)
		{
		    team = Team.Blue;
		    //initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
		    //rotSign = 1f;
		}
		else
		{
		    team = Team.Red;
		    //initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
		    //rotSign = -1f;
		}
		m_GameController = GetComponentInParent<FightingPitEnvController>();
		m_AgentRb = GetComponent<Rigidbody>();
		m_Character = GetComponent<FighterCharacter>();
		m_Animator = gameObject.GetComponent<Animator>();
		
		m_PushBlockSettings = FindObjectOfType<FighterSettings>();
		//MyKey.SetActive(false);
		IHaveAKey = false;
	    }

	    public override void OnEpisodeBegin()
	    {
		//MyKey.SetActive(false);
		IHaveAKey = false;
	    }

	    public override void CollectObservations(VectorSensor sensor)
	    {
		sensor.AddObservation(IHaveAKey);
		sensor.AddObservation(m_AgentRb.velocity.x);
		sensor.AddObservation(m_AgentRb.velocity.y);
		sensor.AddObservation(m_AgentRb.velocity.z);
		Debug.Log(m_AgentRb.velocity);
	    }

	    /// <summary>
	    /// Moves the agent according to the selected action.
	    /// </summary>
	    public void MoveAgent(ActionSegment<int> act)
	    {
		var dirForward = Vector3.zero;
		var dirHorizontal = Vector3.zero;
		var rotateDir = Vector3.zero;

		var forward = act[0];
		var horizontal = act[1];
		var vertical = act[2];
		
		switch (forward)
		{
		    case 1:
		        dirForward = transform.forward * 1f;
		        break;
		    case 2:
		        dirForward = transform.forward * -1f;
		        break;

		}
		switch (horizontal)
		{
		    case 1:
		        dirHorizontal = transform.right * -1f;
		        break;
		    case 2:
		        dirHorizontal = transform.right * 1f;
		        break;

		}
		crouch = false;
		m_Jump = false;
		switch (vertical)
		{
		    case 1:
		        m_Jump = true;
		        crouch = false;
		        break;
		    case 2:
		        m_Jump = false;
		        crouch = true;
		        break;

		}
		var dirToGo = dirForward + dirHorizontal;
		//transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
		//Debug.Log(System.Environment.StackTrace);
		m_Character.Move(dirToGo, crouch, m_Jump);
		//m_AgentRb.AddForce(dirToGo * m_PushBlockSettings.agentRunSpeed, ForceMode.VelocityChange);
	    }

	    /// <summary>
	    /// Called every step of the engine. Here the agent takes an action.
	    /// </summary>
	    public override void OnActionReceived(ActionBuffers actionBuffers)
	    {
		// Move the agent using the action.
		MoveAgent(actionBuffers.DiscreteActions);
	    }

	    void OnCollisionEnter(Collision col)
	    {
		if (col.gameObject.CompareTag("goalArea"))
		{
			Debug.Log("Collided with goal area");
		}
	    /*
		if (col.transform.CompareTag("lock"))
		{
		    if (IHaveAKey)
		    {
		        //MyKey.SetActive(false);
		        IHaveAKey = false;
		        m_GameController.UnlockDoor();
		    }
		}
		if (col.transform.CompareTag("dragon"))
		{
		    m_GameController.KilledByBaddie(this, col);
		    //MyKey.SetActive(false);
		    IHaveAKey = false;
		}
		if (col.transform.CompareTag("portal"))
		{
		    m_GameController.TouchedHazard(this);
		}
		*/
	    }

	    void OnTriggerEnter(Collider col)
	    {
		//if we find a key and it's parent is the main platform we can pick it up
		if (col.transform.CompareTag("key") && col.transform.parent == transform.parent && gameObject.activeInHierarchy)
		{
		    print("Picked up key");
		    //MyKey.SetActive(true);
		    //IHaveAKey = true;
		    //col.gameObject.SetActive(false);
		}
	    }
	    
	    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
	    {
		/*if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")){
		    var branch = 2;
		    var actionsToDisable = new[] {1, 2};
		    actionMask.WriteMask(branch, actionsToDisable);
		}*/
		// same for ml-agents 2.0
		//actionMask.SetActionEnabled(2, 1, false);
	    }
	    
	    public override void Heuristic(in ActionBuffers actionsOut)
	    {
		var discreteActionsOut = actionsOut.DiscreteActions;
		discreteActionsOut[0] = 0;
		discreteActionsOut[1] = 0;
		discreteActionsOut[2] = 0;
		if (Input.GetKey(KeyCode.W))
		{
		    discreteActionsOut[0] = 1;
		}
		else if (Input.GetKey(KeyCode.S))
		{
		    discreteActionsOut[0] = 2;
		}
		else if (Input.GetKey(KeyCode.A))
		{
		    discreteActionsOut[1] = 1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
		    discreteActionsOut[1] = 2;
		}
		else if (Input.GetKey(KeyCode.Space))
		{
		    discreteActionsOut[2] = 1;
		}
		else if (Input.GetKey(KeyCode.C))
		{
		    discreteActionsOut[2] = 2;
		}
	    }
	}
}
