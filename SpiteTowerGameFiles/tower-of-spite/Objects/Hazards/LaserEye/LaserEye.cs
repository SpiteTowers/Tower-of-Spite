using Godot;
using System;

public partial class LaserEye : Node2D, IPlayerTargeter
{
    [Export] public Node2D Target;
    [Export] public float RayLength = 5000f;
    
    [Export] public float DefaultShotTimer = 3f; // in seconds; The default time the Laser Eye waits before shooting at the player
    private float _shotTimer = 3f; // in seconds;
    [Export] public float DefaultShotDelay = 0.5f; // in seconds; The time it takes to wait before shooting at the player
    private float _shotDelay = 0.1f; // in seconds;
    [Export] public float DefaultRecharge = 0.3f;
    private float _rechargeTimer = 0.3f;

    private Area2D _laserCollision;
    private CollisionShape2D _collisionShape;
    private RayCast2D _rayCast;
    private Timer _timer;
    private float _castAngle = 0;
    
    private bool _isTracking = true;
    private bool _isCharging = false;
    private bool _isShooting = false;

    private Line2D _debugLine;


    public void SetTarget(Node2D target)
    {
       Target = target;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       _rayCast = GetNode<RayCast2D>("RayCast2D");
       _rayCast.SetTargetPosition(Vector2.Down * RayLength);
       _rayCast.CollisionMask = 1;
       
       _debugLine = GetNode<Line2D>("DebugLine");
       _debugLine.Show();
       
       _laserCollision = GetNode<Area2D>("LaserCollision");
       _collisionShape = GetNode<CollisionShape2D>("LaserCollision/CollisionShape2D");

       _laserCollision.Monitoring = true;
       _laserCollision.Monitorable = true;

       _laserCollision.SetCollisionLayerValue(3, false);
       _laserCollision.SetCollisionMaskValue(4, true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
       
       // Null Target Protection
       if (Target == null)
       {
          _rayCast.TargetPosition = Vector2.Down * RayLength;
          _isTracking = false;
       }
       else
       {
          _isTracking = true;
       }
       

       if (_isTracking && !_isCharging)
       {
          _aim();
          
          _shotTimer -= (float)delta;
          if (_shotTimer <= 0)
          {
             _isCharging = true;
          }
       }
       else if (_isCharging)
       {
          if (!_isShooting)
          {
             _isShooting = true;
             _shotDelay = DefaultShotDelay;
             _rechargeTimer = DefaultRecharge;
          }
          Shoot(delta);
       }
       else
       {
          _shotTimer = DefaultShotTimer;
       }

       Vector2 start = _rayCast.GlobalPosition;

       Vector2 end;

       if (_rayCast.IsColliding())
       {
          end = _rayCast.GetCollisionPoint();
       }
       else
       {
          end = _rayCast.ToGlobal(_rayCast.TargetPosition);
       }

       Vector2 direction = start.DirectionTo(end);
       float length = start.DistanceTo(end);

       _laserCollision.GlobalPosition = start;
       _laserCollision.GlobalRotation = direction.Angle();

       _collisionShape.Position = Vector2.Zero;
       _collisionShape.Rotation = 0;
       _collisionShape.Scale = Vector2.One;

       SegmentShape2D newShape = new SegmentShape2D();
       newShape.A = Vector2.Zero;
       newShape.B = new Vector2(length, 0);

       _collisionShape.Shape = newShape;

       _debugLine.Points = new Vector2[]
       {
          _debugLine.ToLocal(start),
          _debugLine.ToLocal(end)
       };

       if (_isShooting)
       {
          if (_shotDelay > 0)
          {
             _debugLine.DefaultColor = Colors.Orange;
          }
          else
          {
             _debugLine.DefaultColor = Colors.LimeGreen;
          }
       }
       else
       {
          _debugLine.DefaultColor = _rayCast.IsColliding()
             ? Colors.White
             : Colors.Gray;
       }
    }

    public void Shoot(double delta)
    {
       if (_shotDelay >= 0)
       {
          _shotDelay -= (float)delta;
       }
       else
       {
          _laserCollision.SetCollisionLayerValue(3, true);
       
          if (_rechargeTimer >= 0)
          {
             _rechargeTimer -= (float)delta;
          }
          else
          {
             _laserCollision.SetCollisionLayerValue(3, false);
             _isCharging = false;
             _shotTimer = DefaultShotTimer;
             _isShooting = false;
          }
       }
    }

    private void _aim()
    {
       if (Target == null)
          return;

       Vector2 direction =
          _rayCast.GlobalPosition.DirectionTo(Target.GlobalPosition);

       _rayCast.TargetPosition = direction * RayLength;

       _rayCast.ForceRaycastUpdate();
       
       // TODO: Make main raycast go till it hits ground/wall
       if (_rayCast.IsColliding())
       {
          // GD.Print(RayCast.GetCollider(), RayCast.GetCollisionPoint());
          // DebugLine.SetDefaultColor(Colors.Purple);
       }
    }
}