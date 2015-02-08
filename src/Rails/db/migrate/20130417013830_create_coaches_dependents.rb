class CreateCoachesDependents < ActiveRecord::Migration
  def change
    create_table :coaches_dependents do |t|
      t.integer :dependent_id
      t.integer :coach_id

      t.timestamps
    end
  end
end
