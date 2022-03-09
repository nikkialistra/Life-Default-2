﻿using System;
using ColonistManagement.Selection;
using ColonistManagement.Targeting.Formations;
using Colonists.Services.Selecting;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ColonistManagement.Movement
{
    [RequireComponent(typeof(MovementInput))]
    public class MovementActionInput : MonoBehaviour
    {
        private MovementAction _movementAction = MovementAction.None;

        private MovementInput _movementInput;

        private SelectedColonists _selectedColonists;
        private SelectionInput _selectionInput;

        private GameCursors _gameCursors;

        private PlayerInput _playerInput;

        private InputAction _selectMoveAction;
        private InputAction _selectAttackAction;
        private InputAction _selectHoldAction;
        private InputAction _selectPatrolAction;

        private InputAction _doAction;
        private InputAction _cancelAction;

        [Inject]
        public void Construct(PlayerInput playerInput, SelectedColonists selectedColonists, SelectionInput selectionInput,
            GameCursors gameCursors)
        {
            _playerInput = playerInput;
            _selectedColonists = selectedColonists;
            _selectionInput = selectionInput;
            _gameCursors = gameCursors;
        }

        private void Awake()
        {
            _movementInput = GetComponent<MovementInput>();

            _selectMoveAction = _playerInput.actions.FindAction("Select Move");
            _selectAttackAction = _playerInput.actions.FindAction("Select Attack");
            _selectHoldAction = _playerInput.actions.FindAction("Select Hold");
            _selectPatrolAction = _playerInput.actions.FindAction("Select Patrol");

            _doAction = _playerInput.actions.FindAction("Do");
            _cancelAction = _playerInput.actions.FindAction("Cancel");
        }

        private void OnEnable()
        {
            _selectMoveAction.started += SelectMove;
            _selectAttackAction.started += SelectAttack;
            _selectHoldAction.started += SelectHold;
            _selectPatrolAction.started += SelectPatrol;

            _doAction.started += StartDo;
            _doAction.canceled += Do;

            _cancelAction.started += Cancel;

            _movementInput.MultiCommandReset += Complete;
        }

        private void OnDisable()
        {
            _selectMoveAction.started -= SelectMove;
            _selectAttackAction.started -= SelectAttack;
            _selectHoldAction.started -= SelectHold;
            _selectPatrolAction.started -= SelectPatrol;

            _doAction.started -= StartDo;
            _doAction.canceled -= Do;

            _cancelAction.started -= Cancel;

            _movementInput.MultiCommandReset -= Complete;
        }

        private void SelectMove(InputAction.CallbackContext context)
        {
            if (IfNoColonistsSelected())
            {
                return;
            }

            _movementAction = MovementAction.Move;
            PauseAnotherInput();
            _gameCursors.SetMoveCursor();
        }

        private void SelectAttack(InputAction.CallbackContext context)
        {
            if (IfNoColonistsSelected() || Keyboard.current.altKey.isPressed)
            {
                return;
            }

            _movementAction = MovementAction.Attack;
            PauseAnotherInput();
            _gameCursors.SetAttackCursor();
        }

        private void SelectHold(InputAction.CallbackContext context)
        {
            if (IfNoColonistsSelected())
            {
                return;
            }

            _movementAction = MovementAction.Hold;
            PauseAnotherInput();
        }

        private void SelectPatrol(InputAction.CallbackContext context)
        {
            if (IfNoColonistsSelected())
            {
                return;
            }

            _movementAction = MovementAction.Patrol;
            PauseAnotherInput();
        }

        private bool IfNoColonistsSelected()
        {
            if (_selectedColonists.Colonists.Count == 0)
            {
                return true;
            }

            return false;
        }

        private void PauseAnotherInput()
        {
            _selectionInput.Deactivated = true;
            _movementInput.UnsubscribeFromActions();
        }

        private void StartDo(InputAction.CallbackContext context)
        {
            if (_movementAction == MovementAction.None || !_movementInput.CanTarget)
            {
                return;
            }

            _movementInput.TargetGround(_movementAction == MovementAction.Attack
                ? FormationColor.Red
                : FormationColor.White);
        }

        private void Do(InputAction.CallbackContext context)
        {
            if (_movementAction == MovementAction.None)
            {
                return;
            }

            switch (_movementAction)
            {
                case MovementAction.Move:
                    _movementInput.Move(FormationColor.White);
                    break;
                case MovementAction.Attack:
                    _movementInput.Move(FormationColor.Red);
                    break;
                case MovementAction.Hold:
                    break;
                case MovementAction.Patrol:
                    _movementInput.Move(FormationColor.White);
                    break;
                case MovementAction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!_movementInput.MultiCommand)
            {
                Complete();
            }
        }

        private void Cancel(InputAction.CallbackContext context)
        {
            Complete();
        }

        private void Complete()
        {
            _movementAction = MovementAction.None;
            ResumeAnotherInput();
        }

        private void ResumeAnotherInput()
        {
            _selectionInput.Deactivated = false;
            _movementInput.SubscribeToActions();
            _gameCursors.SetDefaultCursor();
        }
    }
}