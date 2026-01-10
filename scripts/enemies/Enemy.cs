using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract partial class Enemy : BaseCharacter {

    [Export(PropertyHint.None, "suffix:HP")]
    public float HealthPoints {
        get => mHealthPoints;
        set => mHealthPoints = value;
    }
    private float mHealthPoints;

    internal Player mPlayer;

    internal NavigationAgent2D mNavigationAgent2D = null;
    internal NavigationAgent2D mAuxNavAgent = null;

    public override void _Ready() {
        mNavigationAgent2D = GetNode<NavigationAgent2D>("NavigationAgent2D");
        mAuxNavAgent = GetNode<NavigationAgent2D>("AuxNavigationAgent");
    }

    public void Spawn(Vector2 position, Player player) {
        mPlayer = player;
        Position = position;
    }
    public abstract void Attack();

    public abstract void PrepareActions();

    public void TakeDamage(int damage) {
        mHealthPoints -= damage;
    }
}

