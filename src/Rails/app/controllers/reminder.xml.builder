xml.instruct!
xml.Response do
    xml.Gather(:action => @post_to, :numDigits => 1) do
        xml.Say "Hello this is a call from PillBox.  You need to take your medication."
        xml.Say "Please press 1 to repeat this menu. Or press 2 if you are done."
    end
end
