﻿using UnityEngine;
using UnityEngine.UI;
using System;
using MLAPI;
namespace ChampionCommander
{
	public class HexGridBank : MonoBehaviour {

		public int width = 6;
		public int height = 6;

		public Color defaultColor = Color.white;
		public Color touchedColor = Color.magenta;

		public HexCell cellPrefab;
		public Text cellLabelPrefab;

		HexCell[] cells;

		Canvas gridCanvas;
		HexMesh hexMesh;


// player view: cells 1-6 are bank and cells 7-x are arena
// map player view coordinates to shared coordinates
// map shared coordinates to player view coordinates
// global coordinates are unique

		void Awake () {
			gridCanvas = GetComponentInChildren<Canvas>();
			hexMesh = GetComponentInChildren<HexMesh>();

			cells = new HexCell[height * width];

			for (int z = 0, i = 0; z < height; z++) {
				for (int x = 0; x < width; x++) {
					CreateCell(x, z, i++);
				}
			}
		}

		void Start () {
			hexMesh.Triangulate(cells);
			NetworkManager.Singleton.StartHost();
		}

		void Update () {
			if (Input.GetMouseButton(0)) {
				HandleInput();
			}
		}

		void HandleInput () {
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(inputRay, out hit)) {
				TouchCell(hit.point);
			}
		}

		void TouchCell (Vector3 position) {
			position = transform.InverseTransformPoint(position);
			HexCoordinates coordinates = HexCoordinates.FromPosition(position);
			int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
			
			try 
			{
			    HexCell cell = cells[index];
			    cell.color = touchedColor;
			    hexMesh.Triangulate(cells);
			    Debug.Log(index);
    			    Debug.Log(cell.coordinates.ToString());
			    
			}
			catch (IndexOutOfRangeException e)
			{
			  //  Do nothing. This does not concern this grid
			}
			
			if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
                    out var networkedClient))
		        {
		            var player = networkedClient.PlayerObject.GetComponent<Champion>();
		            if (player)
		            {
		                Debug.Log("Player move");
		                player.Move();
		            }
		        }

		}

		void CreateCell (int x, int z, int i) {
			Vector3 position;
			position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
			position.y = 0f;
			position.z = z * (HexMetrics.outerRadius * 1.5f);

			HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
			cell.transform.SetParent(transform, false);
			cell.transform.localPosition = position;
			cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
			cell.color = defaultColor;

			Text label = Instantiate<Text>(cellLabelPrefab);
			label.rectTransform.SetParent(gridCanvas.transform, false);
			label.rectTransform.anchoredPosition =
				new Vector2(position.x, position.z);
			label.text = cell.coordinates.ToStringOnSeparateLines();
		}
	}
}
