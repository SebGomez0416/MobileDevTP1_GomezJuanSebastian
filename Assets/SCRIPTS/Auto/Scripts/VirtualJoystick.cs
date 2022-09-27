using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour , IDragHandler,IPointerUpHandler,IPointerDownHandler
{
   [SerializeField] private RectTransform stick = null;
   [SerializeField] private Image background = null;

   public string player = "";
   [SerializeField] private float limit;


   public void OnPointerDown(PointerEventData eventData)
   {
      background.color=Color.red;
      stick.anchoredPosition = ConverToLocal(eventData);
   }

   public void OnDrag(PointerEventData eventData)
   {
      Vector2 pos = ConverToLocal(eventData);
      if (pos.magnitude > limit)
         pos = pos.normalized * limit;

      stick.anchoredPosition = pos;

      float x = pos.x / limit;
      float y = pos.y / limit;

      setHorizontal(x);
      setVertical(y);
      
   }
   
   public void OnPointerUp(PointerEventData eventData)
   {
      background.color=Color.gray;
      stick.anchoredPosition = Vector2.zero;
      setHorizontal(0);
      setVertical(0);
   }

   private void OnDisable()
   {
      setHorizontal(0);
      setVertical(0);
   }
   private void setHorizontal(float val)
   {
       InputManager.instance.SetAxis("Horizontal"+player,val);
   }
   
   private void setVertical(float val)
   {
       InputManager.instance.SetAxis("Vertical"+player,val);
   }

   private Vector2 ConverToLocal(PointerEventData eventData)
   {
      Vector2 newPos;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(
         transform as RectTransform, 
         eventData.position,
         eventData.enterEventCamera,
         out newPos);
      
      return newPos;
   }

  
}
