using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRGame : MonoBehaviour
{

	[SerializeField] 
	private float m_spawnPos;

	[SerializeField] 
	private float m_destroyPos;

	[SerializeField] 
	private float m_baseSpeed;

	[SerializeField] 
	private float m_maxSpeed;

	private float m_speed;

	[SerializeField]
	private float m_speedIncrementValue;

	[SerializeField] 
	private float m_spawnDecrementValue;

	[SerializeField]
	private float m_maxSpawnRate;

	[SerializeField] 
	private GameObject m_tilePrefab;

	[SerializeField] private float m_progressOnHit;

	private List<string> m_letters = new List<string>();

	[SerializeField] 
	private TaskManager m_taskManager;

	[SerializeField] 
	private float m_baseSpawnRate;

	private float m_spawnRate;

	private bool m_isRunning;

	private float m_spawnTimer;

	private DDRTile m_tileToHit;

	private List<DDRTile> m_tilesInUse = new List<DDRTile>();

	public DDRTile TileToHit
	{
		get { return m_tileToHit; }
		set { m_tileToHit = value; }
	}

	private void Awake()
	{
		m_letters.Add("w");
		m_letters.Add("a");
		m_letters.Add("s");
		m_letters.Add("d");
	}

	private void OnEnable()
	{
		m_isRunning = true;
		m_speed = m_baseSpeed;
		m_spawnRate = m_baseSpawnRate;
		m_spawnTimer = 0f;
		SpawnTile();
	}

	private void OnDisable()
	{
		m_isRunning = false;
		while (m_tilesInUse.Count > 0)
		{
			Destroy(m_tilesInUse[0].gameObject);
			m_tilesInUse.RemoveAt(0);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (!m_isRunning)
		{
			return;
		}
		if (TileToHit != null)
		{
			if (Input.GetKeyDown(TileToHit.Letter))
			{
				m_taskManager.AddProgress(m_progressOnHit);
				if (m_speed < m_maxSpeed)
				{
					m_speed += m_speedIncrementValue;
				}
				if (m_spawnRate > m_maxSpawnRate)
				{
					m_spawnRate -= m_spawnDecrementValue;

				}
				RemoveCurrentTile();				
			}
			else
			{
				for (int i = 0; i < m_letters.Count; i++)
				{
					if (m_letters[i] == TileToHit.Letter)
					{
						continue;
					}
					if (Input.GetKeyDown(m_letters[i]))
					{
						m_speed = m_baseSpeed;
						m_spawnRate = m_baseSpawnRate;
						RemoveCurrentTile();
						break;
					}
				}
			}
		}
	}

	public void SpawnTile()
	{
		DDRTile tile = Instantiate(m_tilePrefab, transform).GetComponent<DDRTile>();
		Vector3 pos = Vector3.zero;
		pos.y = m_spawnPos;
		tile.transform.localPosition = pos;
		tile.Init(m_letters[Random.Range(0, m_letters.Count)]);
		m_tilesInUse.Add(tile);
	}

	private void RemoveCurrentTile()
	{
		m_tilesInUse.Remove(TileToHit);
		Destroy(TileToHit.gameObject);
		TileToHit = null;
	}

	private void LateUpdate()
	{
		if (!m_isRunning)
		{
			return;
		}
		for (int i = 0; i < m_tilesInUse.Count; i++)
		{
			Vector3 pos = m_tilesInUse[i].transform.localPosition;
			if (pos.y < m_destroyPos)
			{
				DDRTile tile = m_tilesInUse[i];
				m_tilesInUse.Remove(tile);
				Destroy(tile.gameObject);
				continue;
			}
			pos.y -= m_speed * Time.deltaTime;
			m_tilesInUse[i].transform.localPosition = pos;
		}
		m_spawnTimer += Time.deltaTime;
		if (m_spawnTimer >= m_spawnRate)
		{
			m_spawnTimer -= m_spawnRate;
			SpawnTile();
		}
	}
}
