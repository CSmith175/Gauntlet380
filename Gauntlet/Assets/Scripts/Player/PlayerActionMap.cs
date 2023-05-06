//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/Player/PlayerActionMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerActionMap : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActionMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActionMap"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""2d1f9f5e-90a2-4be9-b7d0-14365c5c0a5d"",
            ""actions"": [
                {
                    ""name"": ""Player 1"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b5f087ae-ae36-4e35-9372-23db176e8204"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Player 2"",
                    ""type"": ""PassThrough"",
                    ""id"": ""73aef529-6c99-4978-8781-ceca6f80fc83"",
                    ""expectedControlType"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Player 3"",
                    ""type"": ""PassThrough"",
                    ""id"": ""44137564-1e7f-40f9-85bf-6a065e76a070"",
                    ""expectedControlType"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Player 4"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fe3a3f20-2053-4937-856b-07e0bd29a526"",
                    ""expectedControlType"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""1df169c2-0fe6-4cda-85ac-e0708e9e1e86"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""Player 1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""459e3901-1591-45e1-8a26-e08f0636af3f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""daa9c7f8-5aca-49a1-a22d-e900eba6193f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f34ec8bb-8917-4d54-9f9a-c4698a9f043b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cd3076a7-31ea-4d25-8c3a-5300d868d2e3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""601b7898-3d5c-4005-96f8-64e2a8b5639e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4c589cbf-1d1c-480a-88dc-f783a69349fd"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e8c3a0e5-463d-40ec-b9b5-ba1844e1550d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""285a61d1-418d-4b21-8950-ccdc2a6e0aca"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3a8dd40b-521b-45e9-9dc5-7db20088264b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""616adea6-ddeb-41a8-ac4c-cd1eeddcbe7f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 3"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6fb1f578-bf9a-4a37-bd1d-157b5ccb1f02"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""362980f5-74e8-40ef-abdf-4f1d7ccf8840"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aa3e8d3a-ff63-4013-ace9-16e43532ff0f"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3e17d64d-b528-488c-ab1a-9ee6df0a7f72"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""8e157c2f-0d5f-4d17-8539-b914cba8d4a4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 4"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""12d3be9f-2b4a-4f8e-9729-fb6a0b87c616"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fe4d3667-8b2d-4de0-9f4a-1fcc553916b8"",
                    ""path"": ""<Keyboard>/h"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d03365fa-771d-4e0a-9c33-cb930755aec4"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0b84d478-b2be-4b42-acca-1878c2bd639f"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Player 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Player1 = m_PlayerMovement.FindAction("Player 1", throwIfNotFound: true);
        m_PlayerMovement_Player2 = m_PlayerMovement.FindAction("Player 2", throwIfNotFound: true);
        m_PlayerMovement_Player3 = m_PlayerMovement.FindAction("Player 3", throwIfNotFound: true);
        m_PlayerMovement_Player4 = m_PlayerMovement.FindAction("Player 4", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Player1;
    private readonly InputAction m_PlayerMovement_Player2;
    private readonly InputAction m_PlayerMovement_Player3;
    private readonly InputAction m_PlayerMovement_Player4;
    public struct PlayerMovementActions
    {
        private @PlayerActionMap m_Wrapper;
        public PlayerMovementActions(@PlayerActionMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Player1 => m_Wrapper.m_PlayerMovement_Player1;
        public InputAction @Player2 => m_Wrapper.m_PlayerMovement_Player2;
        public InputAction @Player3 => m_Wrapper.m_PlayerMovement_Player3;
        public InputAction @Player4 => m_Wrapper.m_PlayerMovement_Player4;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Player1.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer1;
                @Player1.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer1;
                @Player1.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer1;
                @Player2.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer2;
                @Player2.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer2;
                @Player2.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer2;
                @Player3.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer3;
                @Player3.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer3;
                @Player3.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer3;
                @Player4.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer4;
                @Player4.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer4;
                @Player4.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayer4;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Player1.started += instance.OnPlayer1;
                @Player1.performed += instance.OnPlayer1;
                @Player1.canceled += instance.OnPlayer1;
                @Player2.started += instance.OnPlayer2;
                @Player2.performed += instance.OnPlayer2;
                @Player2.canceled += instance.OnPlayer2;
                @Player3.started += instance.OnPlayer3;
                @Player3.performed += instance.OnPlayer3;
                @Player3.canceled += instance.OnPlayer3;
                @Player4.started += instance.OnPlayer4;
                @Player4.performed += instance.OnPlayer4;
                @Player4.canceled += instance.OnPlayer4;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);
    public interface IPlayerMovementActions
    {
        void OnPlayer1(InputAction.CallbackContext context);
        void OnPlayer2(InputAction.CallbackContext context);
        void OnPlayer3(InputAction.CallbackContext context);
        void OnPlayer4(InputAction.CallbackContext context);
    }
}
