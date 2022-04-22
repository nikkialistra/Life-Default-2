﻿using System;
using System.Collections;
using Common;
using ResourceManagement;
using Sirenix.OdinInspector;
using Units.Ancillaries;
using UnityEngine;

namespace Colonists
{
    [RequireComponent(typeof(ColonistStats))]
    [RequireComponent(typeof(ColonistAnimator))]
    public class ColonistGatherer : MonoBehaviour
    {
        [Required]
        [SerializeField] private UnitEquipment _unitEquipment;
        
        [ValidateInput(nameof(EveryResourceHasDistanceInteraction))]
        [SerializeField] private ResourceInteractionDistanceDictionary _resourceInteractionDistances;
        
        [Space]
        [Required]
        [SerializeField] private FieldOfView _resourceFieldOfView;
        [SerializeField] private float _waitTime = 0.2f;
        
        private Resource _resource;
        
        private Action _onInteractionFinish;

        private ColonistStats _colonistStats;
        
        private ColonistAnimator _animator;

        private Coroutine _watchForExhaustionCoroutine;

        private void Awake()
        {
            _animator = GetComponent<ColonistAnimator>();
            _colonistStats = GetComponent<ColonistStats>();
        }

        public float InteractionDistanceFor(ResourceType resourceType)
        {
            return _resourceInteractionDistances[resourceType];
        }

        public bool CanGather(Resource resource)
        {
            return true;
        }

        public void Gather(Resource resource, Action onInteractionFinish)
        {
            if (_resource == resource)
            {
                return;
            }

            _resource = resource;
            _onInteractionFinish = onInteractionFinish;
            
            _unitEquipment.EquipInstrumentFor(resource.ResourceType);
            _animator.Gather(resource);

            _watchForExhaustionCoroutine = StartCoroutine(WatchForExhaustion());
        }
        
        public void Hit(float passedTime)
        {
            if (_resource == null || _resource.Exhausted)
            {
                FinishGathering();
                return;
            }

            var extractedQuantity = _colonistStats.ResourceDestructionSpeed * passedTime;

            _resource.Extract(extractedQuantity, _colonistStats.ResourceExtractionEfficiency);
            _resource.Hit(transform.position);

            if (_resource.Exhausted)
            {
                _resource = null;
                FinishGathering();
            }
        }

        private IEnumerator WatchForExhaustion()
        {
            while (!_resource.Exhausted)
            {
                yield return new WaitForSeconds(_waitTime);
            }

            _resource = null;
            FinishGathering();
        }

        // Add wait time for cancelling stop gathering if user clicked same resource,
        // and prolongate animation after gathering a little
        public void StopGathering()
        {
            if (_watchForExhaustionCoroutine != null)
            {
                StopCoroutine(_watchForExhaustionCoroutine);
                _watchForExhaustionCoroutine = null;
            }
            
            _resource = null;
            
            StartCoroutine(StopGatheringLater());
        }
        
        public void ToggleResourceFieldOfView()
        {
            _resourceFieldOfView.ToggleDebugShow();
        }

        private IEnumerator StopGatheringLater()
        {
            yield return new WaitForSeconds(_waitTime);

            FinishGathering();
        }

        private void FinishGathering()
        {
            if (_resource != null)
            {
                return;
            }

            if (_watchForExhaustionCoroutine != null)
            {
                StopCoroutine(_watchForExhaustionCoroutine);
                _watchForExhaustionCoroutine = null;
            }
            
            _animator.StopGathering();

            if (_onInteractionFinish != null)
            {
                _onInteractionFinish();
                _onInteractionFinish = null;
            }
        }

        private bool EveryResourceHasDistanceInteraction(ResourceInteractionDistanceDictionary distances, ref string errorMessage)
        {
            foreach (var resourceType in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            {
                if (!distances.ContainsKey(resourceType))
                {
                    errorMessage = $"{resourceType} don't have distance";
                    return false;
                }
            }

            return true;
        }
        
        [Serializable] public class ResourceInteractionDistanceDictionary : SerializableDictionary<ResourceType, float> { }
    }
}
