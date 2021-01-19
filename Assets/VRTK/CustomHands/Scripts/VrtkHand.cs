/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OVRTouchSample;
using Tilia.Interactions.Interactables.Interactors;
using Zinnia.Action;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;

#endif

namespace VRTK
{
    public class VrtkHand : MonoBehaviour
    {
        private const string AnimLayerNamePoint = "Point Layer";
        private const string AnimLayerNameThumb = "Thumb Layer";
        private const string AnimParamNameFlex = "Flex";
        private const string AnimParamNamePose = "Pose";
        private const float ThreshCollisionFlex = 0.9f;
        private const float InputRateChange = 20.0f;

        [SerializeField] private Animator animator = null;
        [SerializeField] private HandPose defaultGrabPose = null;

        [SerializeField] private InteractorFacade grabber;
        [SerializeField] private GameObject poke;

        [SerializeField] private FloatAction primaryIndexTrigger;
        [SerializeField] private BooleanAction primaryThumbButtons;
        [SerializeField] private FloatAction primaryHandTrigger;

        private int _animLayerIndexThumb = -1;
        private int _animLayerIndexPoint = -1;
        private int _animParamIndexFlex = -1;
        private int _animParamIndexPose = -1;

        private bool _isPointing = false;
        private bool _isGivingThumbsUp = false;
        private float _pointBlend = 0.0f;
        private float _thumbsUpBlend = 0.0f;
        private static readonly int Pinch = Animator.StringToHash("Pinch");

        private void Start()
        {
            PokeEnable(false);

            // Get animator layer indices by name, for later use switching between hand visuals
            _animLayerIndexPoint = animator.GetLayerIndex(AnimLayerNamePoint);
            _animLayerIndexThumb = animator.GetLayerIndex(AnimLayerNameThumb);
            _animParamIndexFlex = Animator.StringToHash(AnimParamNameFlex);
            _animParamIndexPose = Animator.StringToHash(AnimParamNamePose);
        }

        private void Update()
        {
            UpdateCapTouchStates();

            _pointBlend = InputValueRateChange(_isPointing, _pointBlend);
            _thumbsUpBlend = InputValueRateChange(_isGivingThumbsUp, _thumbsUpBlend);

            float flex = primaryHandTrigger.Value;

            bool collisionEnabled = grabber.GrabbedObjects.Count == 0 && flex >= ThreshCollisionFlex;
            PokeEnable(_isPointing);

            UpdateAnimStates();
        }

        // Just checking the state of the index and thumb cap touch sensors, but with a little bit of
        // debouncing.
        private void UpdateCapTouchStates()
        {
            _isPointing = primaryIndexTrigger.Value < 0.1f;
            _isGivingThumbsUp = !primaryThumbButtons.Value;
        }

        private float InputValueRateChange(bool isDown, float value)
        {
            float rateDelta = Time.deltaTime * InputRateChange;
            float sign = isDown ? 1.0f : -1.0f;
            return Mathf.Clamp01(value + rateDelta * sign);
        }

        private void UpdateAnimStates()
        {
            bool grabbing = grabber.GrabbedObjects.Count > 0;
            var grabPose = defaultGrabPose;
            if (grabbing)
            {
                var customPose = grabber.GrabbedObjects[0].GetComponent<HandPose>();
                if (customPose) grabPose = customPose;
            }

            // Pose
            HandPoseId handPoseId = grabPose.PoseId;
            animator.SetInteger(_animParamIndexPose, (int) handPoseId);

            // Flex
            // blend between open hand and fully closed fist
            float flex = primaryHandTrigger.Value;
            animator.SetFloat(_animParamIndexFlex, flex);

            // Point
            bool canPoint = !grabbing || grabPose.AllowPointing;
            float point = canPoint ? _pointBlend : 0.0f;
            animator.SetLayerWeight(_animLayerIndexPoint, point);

            // Thumbs up
            bool canThumbsUp = !grabbing || grabPose.AllowThumbsUp;
            float thumbsUp = canThumbsUp ? _thumbsUpBlend : 0.0f;
            animator.SetLayerWeight(_animLayerIndexThumb, thumbsUp);

            float pinch = primaryIndexTrigger.Value;
            animator.SetFloat(Pinch, pinch);
        }

        private void PokeEnable(bool enabled)
        {
            poke.SetActive(enabled);
        }
    }
}