using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
 using TMPro;
 
 public class FishingQTE : MonoBehaviour
 {
     public RectTransform bar; 
     public float barSpeed = 500f; 
     public float successZoneStart = 0.4f; 
     public float successZoneEnd = 0.6f; 
 
     public List<string> collectibleItems; 
     public TextMeshProUGUI feedbackText; 
     public GameObject fishingUI; 
 
     private float barPosition = 0f; 
     private bool movingRight = true; 
     private bool canClick = false; 
     private int successCount = 0;  
     private int maxSuccesses = 10; 
 
     private CollectionManager collectionManager;
 
     private void Start()
     {
         collectionManager = FindObjectOfType<CollectionManager>(); 
         feedbackText.text = "";
         StartCoroutine(MoveBar()); 
     }
 
     private void Update()
     {
         
         if (canClick && Input.GetMouseButtonDown(0)) 
         {
             float normalizedBarPosition = Mathf.InverseLerp(-400f, 400f, barPosition);
             if (normalizedBarPosition >= successZoneStart && normalizedBarPosition <= successZoneEnd)
             { 
                 TryFishing(); 
             }
             else
             {
                 feedbackText.text = "Missed! Try Again!"; 
                 StartCoroutine(ClearFeedbackText());
             }
         }
         
     }
 
     private void TryFishing()
     {
         if (successCount >= maxSuccesses)
         {
             EndFishingMinigame();
             return;
         }
 
         if (Random.value < 0.25f) // 25% chance to catch trash
         {
             feedbackText.text = "You caught Trash...";
         }
         else
         {
             if (collectibleItems.Count > 0)
             {
                 string randomItem = collectibleItems[Random.Range(0, collectibleItems.Count)];
                 if (collectionManager != null)
                 {
                     collectionManager.CollectItem(randomItem);
                 }
 
                 feedbackText.text = $"Caught: {randomItem}!";
             }
         }
 
         successCount++; // Increase success count
 
         if (successCount >= maxSuccesses)
         {
             EndFishingMinigame();
         }
 
         StartCoroutine(ClearFeedbackText());
     }
 
     private void EndFishingMinigame()
     {
         Debug.Log("Fishing mini-game finished!");
         fishingUI.SetActive(false); 
     }
 
     private IEnumerator MoveBar()
     {
         canClick = barPosition >= Mathf.Lerp(-400f, 400f, successZoneStart) &&
                    barPosition <= Mathf.Lerp(-400f, 400f, successZoneEnd);
         while (true)
         {
             if (movingRight)
             {
                 barPosition += barSpeed * Time.deltaTime * 3 ;
                 if (barPosition >= 400f)
                 {
                     barPosition = 400f;
                     movingRight = false;
                 }
             }
             else
             {
                 barPosition -= barSpeed * Time.deltaTime * 3 ;
                 if (barPosition <= -400f)
                 {
                     barPosition = -400f;
                     movingRight = true;
                 }
             }
 
             bar.anchoredPosition = new Vector2(barPosition, bar.anchoredPosition.y);
             canClick = barPosition >= successZoneStart * 100f && barPosition <= successZoneEnd * 100f;
 
             yield return null;
         }
     }
 
     private IEnumerator ClearFeedbackText()
     {
         yield return new WaitForSeconds(1f);
         feedbackText.text = "";
     }
     
     void OnDrawGizmos()
     {
         if (bar == null) return; // Prevent errors if bar is not assigned
     
         Gizmos.color = Color.green;
     
         // Convert success zone values (0-1) into local UI space (anchored position)
         float startX = Mathf.Lerp(-bar.rect.width / 2, bar.rect.width / 2, successZoneStart);
         float endX = Mathf.Lerp(-bar.rect.width / 2, bar.rect.width / 2, successZoneEnd);
     
         // Convert local UI space to world space
         Vector3 startWorld = bar.transform.TransformPoint(new Vector3(startX, 0, 0));
         Vector3 endWorld = bar.transform.TransformPoint(new Vector3(endX, 0, 0));
     
         // Draw vertical lines marking the success zone in world space
         Gizmos.DrawLine(startWorld + Vector3.down * 50, startWorld + Vector3.up * 50);
         Gizmos.DrawLine(endWorld + Vector3.down * 50, endWorld + Vector3.up * 50);
     }
     
 }
