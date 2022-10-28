using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.U2D;

namespace UnityEngine.UI
{
    [Icon("Assets/Space WebUI/Rounded Corners/Images/roundedCorners_icon.png")]
    [HelpURL("http://sergiom.dev/rpg/ficha")]

    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(Mask))]
    [AddComponentMenu("UI/Rounded Edges", 11)]

    public class RoundedCorners : MaskableGraphic, ISerializationCallbackReceiver, ILayoutElement, ICanvasRaycastFilter
    {
        static protected Material s_ETC1DefaultUI = null;

        [FormerlySerializedAs("m_Frame")]
        public Sprite m_Sprite;

        public Sprite sprite
        {
            get { return m_Sprite; }
            set
            {
                if (m_Sprite != null)
                {
                    if (m_Sprite != value)
                    {
                        m_SkipLayoutUpdate = m_Sprite.rect.size.Equals(value ? value.rect.size : Vector2.zero);
                        m_SkipMaterialUpdate = m_Sprite.texture == (value ? value.texture : null);
                        m_Sprite = value;

                        SetAllDirty();
                        TrackSprite();
                    }
                }
                else if (value != null)
                {
                    m_SkipLayoutUpdate = value.rect.size == Vector2.zero;
                    m_SkipMaterialUpdate = value.texture == null;
                    m_Sprite = value;

                    SetAllDirty();
                    TrackSprite();
                }
            }
        }

        [NonSerialized]
        private Sprite m_OverrideSprite;

        private Sprite activeSprite { get { return m_OverrideSprite != null ? m_OverrideSprite : sprite; } }
        
        // Not serialized until we support read-enabled sprites better.
        private float m_AlphaHitTestMinimumThreshold = 0;

        // Whether this is being tracked for Atlas Binding.
        private bool m_Tracked = false;

        public float alphaHitTestMinimumThreshold { get { return m_AlphaHitTestMinimumThreshold; } set { m_AlphaHitTestMinimumThreshold = value; } }
        
        protected RoundedCorners()
        {
            useLegacyMeshGeneration = false;
        }
        
        static public Material defaultETC1GraphicMaterial
        {
            get
            {
                if (s_ETC1DefaultUI == null)
                    s_ETC1DefaultUI = Canvas.GetETC1SupportedCanvasMaterial();
                return s_ETC1DefaultUI;
            }
        }

        /// <summary>
        /// Image's texture comes from the UnityEngine.Image.
        /// </summary>
        public override Texture mainTexture
        {
            get
            {
                if (activeSprite == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }

                return activeSprite.texture;
            }
        }

        /// <summary>
        /// Whether the Sprite of the image has a border to work with.
        /// </summary>

        public bool hasBorder
        {
            get
            {
                if (activeSprite != null)
                {
                    Vector4 v = activeSprite.border;
                    return v.sqrMagnitude > 0f;
                }
                return false;
            }
        }

        [SerializeField] [Range(0,90)] public int TopLeft = 90, TopRight = 90, BottomLeft = 90, BottomRight = 90;
        [SerializeField] public Vector4 m_CornerAngles => new Vector4(TopLeft,TopRight,BottomLeft,BottomRight);

        private float angleFunction(float a)
        {
            if(a > 0) return 6.2f * Mathf.Pow(.6f, -5+(a/10))+.1f;
            else return 1000;
        }

        // case 1066689 cache referencePixelsPerUnit when canvas parent is disabled;
        private float m_CachedReferencePixelsPerUnit = 100;

        public float pixelsPerUnit
        {
            get
            {
                float spritePixelsPerUnit = 100;
                if (activeSprite)
                    spritePixelsPerUnit = activeSprite.pixelsPerUnit;

                if (canvas)
                    m_CachedReferencePixelsPerUnit = canvas.referencePixelsPerUnit;

                return spritePixelsPerUnit / m_CachedReferencePixelsPerUnit;
            }
        }

        protected Vector4 cornerAnglesPixels => new Vector4(angleFunction(m_CornerAngles.x), angleFunction(m_CornerAngles.y), angleFunction(m_CornerAngles.z), angleFunction(m_CornerAngles.w));
        protected Vector4 cornerAngles
        {
            get { return cornerAnglesPixels * pixelsPerUnit; }
        }

        /// <summary>
        /// The specified Material used by this Image. The default Material is used instead if one wasn't specified.
        /// </summary>
        public override Material material
        {
            get
            {
                if (m_Material != null)
                    return m_Material;
#if UNITY_EDITOR
                if (Application.isPlaying && activeSprite && activeSprite.associatedAlphaSplitTexture != null)
                    return defaultETC1GraphicMaterial;
#else

                if (activeSprite && activeSprite.associatedAlphaSplitTexture != null)
                    return defaultETC1GraphicMaterial;
#endif

                return defaultMaterial;
            }

            set
            {
                base.material = value;
            }
        }

        /// <summary>
        /// See ISerializationCallbackReceiver.
        /// </summary>
        public virtual void OnBeforeSerialize() {}

        /// <summary>
        /// See ISerializationCallbackReceiver.
        /// </summary>
        public virtual void OnAfterDeserialize() {}

        private void PreserveSpriteAspectRatio(ref Rect rect, Vector2 spriteSize)
        {
            var spriteRatio = spriteSize.x / spriteSize.y;
            var rectRatio = rect.width / rect.height;

            if (spriteRatio > rectRatio)
            {
                var oldHeight = rect.height;
                rect.height = rect.width * (1.0f / spriteRatio);
                rect.y += (oldHeight - rect.height) * rectTransform.pivot.y;
            }
            else
            {
                var oldWidth = rect.width;
                rect.width = rect.height * spriteRatio;
                rect.x += (oldWidth - rect.width) * rectTransform.pivot.x;
            }
        }

        /// Image's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            var padding = activeSprite == null ? Vector4.zero : Sprites.DataUtility.GetPadding(activeSprite);
            var size = activeSprite == null ? Vector2.zero : new Vector2(activeSprite.rect.width, activeSprite.rect.height);

            Rect r = GetPixelAdjustedRect();
            // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            int spriteW = Mathf.RoundToInt(size.x);
            int spriteH = Mathf.RoundToInt(size.y);

            var v = new Vector4(
                padding.x / spriteW,
                padding.y / spriteH,
                (spriteW - padding.z) / spriteW,
                (spriteH - padding.w) / spriteH);

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f)
            {
                PreserveSpriteAspectRatio(ref r, size);
            }

            v = new Vector4(
                r.x + r.width * v.x,
                r.y + r.height * v.y,
                r.x + r.width * v.z,
                r.y + r.height * v.w
            );

            return v;
        }

        /// <summary>
        /// Adjusts the image size to make it pixel-perfect.
        /// </summary>
        /// <remarks>
        /// This means setting the Images RectTransform.sizeDelta to be equal to the Sprite dimensions.
        /// </remarks>
        public override void SetNativeSize()
        {
            if (activeSprite != null)
            {
                float w = activeSprite.rect.width / pixelsPerUnit;
                float h = activeSprite.rect.height / pixelsPerUnit;
                rectTransform.anchorMax = rectTransform.anchorMin;
                rectTransform.sizeDelta = new Vector2(w, h);
                SetAllDirty();
            }
        }

        /// <summary>
        /// Update the UI renderer mesh.
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (activeSprite == null)
            {
                base.OnPopulateMesh(toFill);
                return;
            }
            
            GenerateSlicedSprite(toFill);
        }

        public void CornerUpdate()
        {

        }

        private void TrackSprite()
        {
            if (activeSprite != null && activeSprite.texture == null)
            {
                TrackImage(this);
                m_Tracked = true;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            TrackSprite();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (m_Tracked)
                UnTrackImage(this);
        }

        /// <summary>
        /// Update the renderer's material.
        /// </summary>

        protected override void UpdateMaterial()
        {
            base.UpdateMaterial();

            // check if this sprite has an associated alpha texture (generated when splitting RGBA = RGB + A as two textures without alpha)

            if (activeSprite == null)
            {
                canvasRenderer.SetAlphaTexture(null);
                return;
            }

            Texture2D alphaTex = activeSprite.associatedAlphaSplitTexture;

            if (alphaTex != null)
            {
                canvasRenderer.SetAlphaTexture(alphaTex);
            }
        }

        protected override void OnCanvasHierarchyChanged()
        {
            base.OnCanvasHierarchyChanged();
            if (canvas == null)
            {
                m_CachedReferencePixelsPerUnit = 100;
            }
            else if (canvas.referencePixelsPerUnit != m_CachedReferencePixelsPerUnit)
            {
                SetVerticesDirty();
                SetLayoutDirty();
            }
        }

        /// <summary>
        /// Generate vertices for a simple Image.
        /// </summary>
        void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
        {
            Vector4 v = GetDrawingDimensions(lPreserveAspect);
            var uv = (activeSprite != null) ? Sprites.DataUtility.GetOuterUV(activeSprite) : Vector4.zero;

            var color32 = color;
            vh.Clear();
            vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
            vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
            vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
            vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
        }

        static readonly Vector2[] s_VertScratchTL = new Vector2[4];
        static Vector2[] s_VertScratchTR = new Vector2[4];
        static Vector2[] s_VertScratchBL = new Vector2[4];
        static Vector2[] s_VertScratchBR = new Vector2[4];
        
        static readonly Vector2[] s_UVScratch = new Vector2[4];

        /// <summary>
        /// Generate vertices for a 9-sliced Image.
        /// </summary>
        private void GenerateSlicedSprite(VertexHelper toFill)
        {
            if (!hasBorder)
            {
                GenerateSimpleSprite(toFill, false);
                return;
            }

            Vector4 outer, inner, padding, border;

            if (activeSprite != null)
            {
                outer = Sprites.DataUtility.GetOuterUV(activeSprite);
                inner = Sprites.DataUtility.GetInnerUV(activeSprite);
                padding = Sprites.DataUtility.GetPadding(activeSprite);
                border = activeSprite.border;
            }
            else
            {
                outer = Vector4.zero;
                inner = Vector4.zero;
                padding = Vector4.zero;
                border = Vector4.zero;
            }

            Rect rect = GetPixelAdjustedRect();

            #region Multiplied Pixels Per Unit

            Vector4 adjustedBordersTL = GetAdjustedBorders(border / cornerAngles.x, rect);
            Vector4 adjustedBordersTR = GetAdjustedBorders(border / cornerAngles.y, rect);
            Vector4 adjustedBordersBL = GetAdjustedBorders(border / cornerAngles.z, rect);
            Vector4 adjustedBordersBR = GetAdjustedBorders(border / cornerAngles.w, rect);

            Vector4 paddidngTL = padding / cornerAngles.x;
            Vector4 paddidngTR = padding / cornerAngles.y;
            Vector4 paddidngBL = padding / cornerAngles.z;
            Vector4 paddidngBR = padding / cornerAngles.w;

            s_VertScratchTL[0] = new Vector2(paddidngTL.x, paddidngTL.y);
            s_VertScratchTR[0] = new Vector2(paddidngTR.x, paddidngTR.y);
            s_VertScratchBL[0] = new Vector2(paddidngBL.x, paddidngBL.y);
            s_VertScratchBR[0] = new Vector2(padding.x, padding.y);
            
            s_VertScratchTL[3] = new Vector2(rect.width - paddidngTL.z, rect.height - paddidngTL.w);
            s_VertScratchTR[3] = new Vector2(rect.width - paddidngTR.z, rect.height - paddidngTR.w);
            s_VertScratchBL[3] = new Vector2(rect.width - paddidngBL.z, rect.height - paddidngBL.w);
            s_VertScratchBR[3] = new Vector2(rect.width - paddidngBR.z, rect.height - paddidngBR.w);

            //Top Left
            s_VertScratchTL[1].x = adjustedBordersTL.x;
            s_VertScratchTL[1].y = adjustedBordersTL.y;

            s_VertScratchTL[2].x = rect.width - adjustedBordersTL.z;
            s_VertScratchTL[2].y = rect.height - adjustedBordersTL.w;
            
            //Top Right
            s_VertScratchTR[1].x = adjustedBordersTR.x;
            s_VertScratchTR[1].y = adjustedBordersTR.y;

            s_VertScratchTR[2].x = rect.width - adjustedBordersTR.z;
            s_VertScratchTR[2].y = rect.height - adjustedBordersTR.w;
            
            //Bottom Left
            s_VertScratchBL[1].x = adjustedBordersBL.x;
            s_VertScratchBL[1].y = adjustedBordersBL.y;

            s_VertScratchBL[2].x = rect.width - adjustedBordersBL.z;
            s_VertScratchBL[2].y = rect.height - adjustedBordersBL.w;
            
            //Bottom Right
            s_VertScratchBR[1].x = adjustedBordersBR.x;
            s_VertScratchBR[1].y = adjustedBordersBR.y;

            s_VertScratchBR[2].x = rect.width - adjustedBordersBR.z;
            s_VertScratchBR[2].y = rect.height - adjustedBordersBR.w;

            #endregion

            for (int i = 0; i < 4; ++i)
            {
                s_VertScratchTL[i].x += rect.x;
                s_VertScratchTL[i].y += rect.y;
                
                s_VertScratchTR[i].x += rect.x;
                s_VertScratchTR[i].y += rect.y;

                s_VertScratchBL[i].x += rect.x;
                s_VertScratchBL[i].y += rect.y;
                
                s_VertScratchBR[i].x += rect.x;
                s_VertScratchBR[i].y += rect.y;
            }

            s_UVScratch[0] = new Vector2(outer.x, outer.y);
            s_UVScratch[1] = new Vector2(inner.x, inner.y);
            s_UVScratch[2] = new Vector2(inner.z, inner.w);
            s_UVScratch[3] = new Vector2(outer.z, outer.w);

            toFill.Clear();

            CornerAddQuad(toFill, s_VertScratchBL, new Vector4(0,0,1,1)); //BL
            HorizontalAddQuad(toFill, s_VertScratchBL, s_VertScratchBR, new Vector4(1,0,2,1)); //HB
            CornerAddQuad(toFill, s_VertScratchBR, new Vector4(2,0,3,1)); //BR
            VerticalAddQuad(toFill, s_VertScratchBL, s_VertScratchTL, new Vector4(0,1,1,2)); //VL
            CenterAddQuad(toFill, s_VertScratchTL, s_VertScratchTR, s_VertScratchBL, s_VertScratchBR); //Center
            VerticalAddQuad(toFill, s_VertScratchBR, s_VertScratchTR, new Vector4(2, 1,3,2)); //VR
            CornerAddQuad(toFill, s_VertScratchTL, new Vector4(0,2,1,3)); //TL
            CornerAddQuad(toFill, s_VertScratchTR, new Vector4(2,2,3,3)); //TR
            HorizontalAddQuad(toFill, s_VertScratchTL, s_VertScratchTR, new Vector4(1,2,2,3)); //HT
        }

        private void CornerAddQuad(VertexHelper toFill, Vector2[] Vert, Vector4 xy)
        {
            AddQuadCorner(toFill,
                new Vector2(Vert[(int)xy.x].x, Vert[(int)xy.y].y),
                new Vector2(Vert[(int)xy.z].x, Vert[(int)xy.w].y),
                this.color, 
                new Vector2(s_UVScratch[(int)xy.x].x, s_UVScratch[(int)xy.y].y),
                new Vector2(s_UVScratch[(int)xy.z].x, s_UVScratch[(int)xy.w].y));
        }
        
        private void HorizontalAddQuad(VertexHelper toFill, Vector2[] Vert, Vector2[] Vert2, Vector4 xy)
        {
            AddQuadHorizontal(toFill,
                new Vector2(Vert[(int)xy.x].x, Vert[(int)xy.y].y),
                new Vector2(Vert[(int)xy.z].x, Vert[(int)xy.w].y),
                new Vector2(Vert2[(int)xy.x].x, Vert2[(int)xy.y].y),
                new Vector2(Vert2[(int)xy.z].x, Vert2[(int)xy.w].y),
                this.color, 
                new Vector2(s_UVScratch[(int)xy.x].x, s_UVScratch[(int)xy.y].y),
                new Vector2(s_UVScratch[(int)xy.z].x, s_UVScratch[(int)xy.w].y));
        }
        
        private void VerticalAddQuad(VertexHelper toFill, Vector2[] Vert, Vector2[] Vert2, Vector4 xy)
        {
            AddQuadVertical(toFill,
                new Vector2(Vert[(int)xy.x].x, Vert[(int)xy.y].y),
                new Vector2(Vert[(int)xy.z].x, Vert[(int)xy.w].y),
                new Vector2(Vert2[(int)xy.x].x, Vert2[(int)xy.y].y),
                new Vector2(Vert2[(int)xy.z].x, Vert2[(int)xy.w].y),
                this.color, 
                new Vector2(s_UVScratch[(int)xy.x].x, s_UVScratch[(int)xy.y].y),
                new Vector2(s_UVScratch[(int)xy.z].x, s_UVScratch[(int)xy.w].y));
        }

        private void CenterAddQuad(VertexHelper toFill, Vector2[] Vert, Vector2[] Vert2, Vector2[] Vert3, Vector2[] Vert4)
        {
            AddQuadCenter(toFill,
                new Vector2(Vert4[2].x, Vert4[1].y),
                new Vector2(Vert2[2].x, Vert2[2].y),
                new Vector2(Vert[1].x, Vert[2].y),
                new Vector2(Vert3[1].x, Vert3[1].y),
                this.color, 
                new Vector2(s_UVScratch[1].x, s_UVScratch[1].y),
                new Vector2(s_UVScratch[2].x, s_UVScratch[2].y));
        }
        
        static void AddQuadCorner(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
        
        static void AddQuadHorizontal(VertexHelper vertexHelper, Vector2 posMin1, Vector2 posMax1, Vector2 posMin2, Vector2 posMax2, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin1.x, posMin1.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin1.x, posMax1.y, 0), color, new Vector2(uvMax.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMax2.x, posMax2.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax2.x, posMin2.y, 0), color, new Vector2(uvMax.x, uvMax.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
        
        static void AddQuadVertical(VertexHelper vertexHelper, Vector2 posMin1, Vector2 posMax1, Vector2 posMin2, Vector2 posMax2, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin1.x, posMin1.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin2.x, posMax2.y, 0), color, new Vector2(uvMax.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMax2.x, posMax2.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax1.x, posMin1.y, 0), color, new Vector2(uvMax.x, uvMax.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
        
        static void AddQuadCenter(VertexHelper vertexHelper, Vector2 posVert1, Vector2 posVert2, Vector2 posVert3, Vector2 posVert4, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posVert1.x, posVert1.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posVert2.x, posVert2.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posVert3.x, posVert3.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posVert4.x, posVert4.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
        {
            Rect originalRect = rectTransform.rect;

            for (int axis = 0; axis <= 1; axis++)
            {
                float borderScaleRatio;
                
                if (originalRect.size[axis] != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / originalRect.size[axis];
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }

                float combinedBorders = border[axis] + border[axis + 2];
                if (adjustedRect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }
            return border;
        }
        
        /// <summary>
        /// See ILayoutElement.CalculateLayoutInputHorizontal.
        /// </summary>
        public virtual void CalculateLayoutInputHorizontal() {}

        /// <summary>
        /// See ILayoutElement.CalculateLayoutInputVertical.
        /// </summary>
        public virtual void CalculateLayoutInputVertical() {}

        /// <summary>
        /// See ILayoutElement.minWidth.
        /// </summary>
        public virtual float minWidth { get { return 0; } }

        /// <summary>
        /// If there is a sprite being rendered returns the size of that sprite.
        /// In the case of a slided or tiled sprite will return the calculated minimum size possible
        /// </summary>
        public virtual float preferredWidth
        {
            get
            {
                if (activeSprite == null)
                    return 0;

                return Sprites.DataUtility.GetMinSize(activeSprite).x / pixelsPerUnit;
            }
        }

        /// <summary>
        /// See ILayoutElement.flexibleWidth.
        /// </summary>
        public virtual float flexibleWidth { get { return -1; } }

        /// <summary>
        /// See ILayoutElement.minHeight.
        /// </summary>
        public virtual float minHeight { get { return 0; } }

        /// <summary>
        /// If there is a sprite being rendered returns the size of that sprite.
        /// In the case of a slided or tiled sprite will return the calculated minimum size possible
        /// </summary>
        public virtual float preferredHeight
        {
            get
            {
                if (activeSprite == null)
                    return 0;

                return Sprites.DataUtility.GetMinSize(activeSprite).y / pixelsPerUnit;
            }
        }

        /// <summary>
        /// See ILayoutElement.flexibleHeight.
        /// </summary>
        public virtual float flexibleHeight { get { return -1; } }

        /// <summary>
        /// See ILayoutElement.layoutPriority.
        /// </summary>
        public virtual int layoutPriority { get { return 0; } }

        /// <summary>
        /// Calculate if the ray location for this image is a valid hit location. Takes into account a Alpha test threshold.
        /// </summary>
        /// <param name="screenPoint">The screen point to check against</param>
        /// <param name="eventCamera">The camera in which to use to calculate the coordinating position</param>
        /// <returns>If the location is a valid hit or not.</returns>
        /// <remarks> Also see See:ICanvasRaycastFilter.</remarks>
        public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (alphaHitTestMinimumThreshold <= 0)
                return true;

            if (alphaHitTestMinimumThreshold > 1)
                return false;

            if (activeSprite == null)
                return true;

            Vector2 local;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local))
                return false;

            Rect rect = GetPixelAdjustedRect();

            // Convert to have lower left corner as reference point.
            local.x += rectTransform.pivot.x * rect.width;
            local.y += rectTransform.pivot.y * rect.height;

            local = MapCoordinate(local, rect);

            // Convert local coordinates to texture space.
            Rect spriteRect = activeSprite.textureRect;
            float x = (spriteRect.x + local.x) / activeSprite.texture.width;
            float y = (spriteRect.y + local.y) / activeSprite.texture.height;

            try
            {
                return activeSprite.texture.GetPixelBilinear(x, y).a >= alphaHitTestMinimumThreshold;
            }
            catch (UnityException e)
            {
                Debug.LogError("Using alphaHitTestMinimumThreshold greater than 0 on Image whose sprite texture cannot be read. " + e.Message + " Also make sure to disable sprite packing for this sprite.", this);
                return true;
            }
        }

        private Vector2 MapCoordinate(Vector2 local, Rect rect)
        {
            Rect spriteRect = activeSprite.rect;

            Vector4 border = activeSprite.border;
            Vector4 adjustedBorder = GetAdjustedBorders(border / pixelsPerUnit, rect);

            for (int i = 0; i < 2; i++)
            {
                if (local[i] <= adjustedBorder[i])
                    continue;

                if (rect.size[i] - local[i] <= adjustedBorder[i + 2])
                {
                    local[i] -= (rect.size[i] - spriteRect.size[i]);
                    continue;
                }

                float lerp = Mathf.InverseLerp(adjustedBorder[i], rect.size[i] - adjustedBorder[i + 2], local[i]);
                local[i] = Mathf.Lerp(border[i], spriteRect.size[i] - border[i + 2], lerp);
            }

            return local;
        }

        // To track textureless images, which will be rebuild if sprite atlas manager registered a Sprite Atlas that will give this image new texture
        static List<RoundedCorners> m_TrackedTexturelessImages = new List<RoundedCorners>();
        static bool s_Initialized;

        static void RebuildImage(SpriteAtlas spriteAtlas)
        {
            for (var i = m_TrackedTexturelessImages.Count - 1; i >= 0; i--)
            {
                var g = m_TrackedTexturelessImages[i];
                if (null != g.activeSprite && spriteAtlas.CanBindTo(g.activeSprite))
                {
                    g.SetAllDirty();
                    m_TrackedTexturelessImages.RemoveAt(i);
                }
            }
        }

        private static void TrackImage(RoundedCorners g)
        {
            if (!s_Initialized)
            {
                SpriteAtlasManager.atlasRegistered += RebuildImage;
                s_Initialized = true;
            }

            m_TrackedTexturelessImages.Add(g);
        }

        private static void UnTrackImage(RoundedCorners g)
        {
            m_TrackedTexturelessImages.Remove(g);
        }

        protected override void OnDidApplyAnimationProperties()
        {
            SetMaterialDirty();
            SetVerticesDirty();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            TopLeft = Mathf.Clamp(TopLeft, 0, 90);
            TopRight = Mathf.Clamp(TopRight, 0, 90);
            BottomLeft = Mathf.Clamp(BottomLeft, 0, 90);
            BottomRight = Mathf.Clamp(BottomRight, 0, 90);
        }

#endif
    }
}
